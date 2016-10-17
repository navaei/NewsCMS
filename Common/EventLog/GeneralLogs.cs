using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Tazeyab.Common.Models;

namespace Tazeyab.Common.EventsLog
{
    public class GeneralLogs
    {
        static int MaxBufferSize = 500;
        static bool StackOverFlow;
        static List<LogsBuffer> buffers = new List<LogsBuffer>();
        static Dictionary<TypeOfLog, string> LogsColor = new Dictionary<TypeOfLog, string>() { { TypeOfLog.End, "Black" }, { TypeOfLog.Error, "Red" },
        { TypeOfLog.Info, "Black" }, { TypeOfLog.OK, "Green" }
        , { TypeOfLog.Other, "Gray" }, { TypeOfLog.Start, "Blue" }};

        public static List<LogsBuffer> getLogs()
        {
            //LogsBuffer[] buffersTemp = buffers.ToArray();
            // buffers.Clear();
            return buffers;
        }
        public static List<string> getNewLogsAsHTML()
        {
            return getLogs().Select(x => string.Format("[span style='color:{0}'] {1}: {2} | {3} [/span]",
                LogsColor[x.Type], x.CreationDate.ToShortTimeString(), x.Type, x.Value)).ToList();
        }
        public static List<string> getNewLogs()
        {
            return getLogs().Select(x => x.CreationDate.ToShortTimeString() + ": " + x.Type + "|" + x.Value).ToList();
        }
        public static void ClearCache()
        {
            buffers.Clear();
        }
        #region WriteLog
        [Obsolete("WriteLog is deprecated, please use WriteLog by 3 args instead.")]
        public static void WriteLog(string value)
        {
            LogsBuffer log = new LogsBuffer();
            log.Value = value;
            log.CreationDate = DateTime.Now;
            if (value.StartsWith("OK"))
                log.Type = TypeOfLog.OK;
            else if (value.StartsWith("INFO"))
                log.Type = TypeOfLog.Info;
            else if (value.StartsWith("Error"))
                log.Type = TypeOfLog.Error;

            WriteLogToRam(log);
        }
        public static void WriteLog(Exception ex, Type classType)
        {
            WriteLog(ex.InnerException == null ? ex.Message.SubstringM(0, 256) : ex.InnerException.Message.SubstringM(0, 256), TypeOfLog.Error, classType);
        }
        public static void WriteLog(string value, TypeOfLog type)
        {
            WriteLog(value, type, string.Empty);
        }
        public static void WriteLog(string value, TypeOfLog type, Type classType)
        {
            WriteLog(value, type, classType.Name);
        }
        public static void WriteLog(string value, TypeOfLog type, string code)
        {
            WriteLogToRam(new LogsBuffer { CreationDate = DateTime.Now, Type = type, Value = value, Code = code });
        }
        private static void WriteLogToRam(LogsBuffer log)
        {
            Console.ForegroundColor = log.Type == TypeOfLog.Error ? ConsoleColor.Red
                : log.Type == TypeOfLog.OK ? ConsoleColor.Green
                : log.Type == TypeOfLog.Start || log.Type == TypeOfLog.End ? ConsoleColor.Magenta
                : log.Type == TypeOfLog.Warning ? ConsoleColor.Yellow : ConsoleColor.White;

            Console.WriteLine(log.Type.ToString() + ":" + log.Code + ":" + log.Value);

            buffers.Add(log);
            if (buffers.Count > MaxBufferSize)
            {
                StackOverFlow = true;
                buffers.Clear();
            }

        }
        public static void WriteLogInDB(string Log)
        {
            WriteLogInDB(Log, TypeOfLog.Other);
        }
        public static void WriteLogInDB(string value, TypeOfLog type)
        {
            WriteLogInDB(value, type, string.Empty);
        }
        public static void WriteLogInDB(Exception ex)
        {
            WriteLogInDB(ex.InnerException == null ? ex.Message.SubstringM(0, 256) : ex.InnerException.Message.SubstringM(0, 256), TypeOfLog.Error);
        }
        public static void WriteLogInDB(Exception ex, Type classType)
        {
            WriteLogInDB(ex.InnerException == null ? ex.Message.SubstringM(0, 256) : ex.InnerException.Message.SubstringM(0, 256), TypeOfLog.Error, classType.Name);
        }
        public static void WriteLogInDB(string value, TypeOfLog type, Type classType)
        {
            WriteLogInDB(value, type, classType.Name);
        }
        public static void WriteLogInDB(string value, TypeOfLog type, string code)
        {
            WriteLog(value, type, code);

            ServiceFactory.Get<ILogsBusiness>().Create(new Tazeyab.Common.Models.LogsBuffer
            {
                Type = type,
                CreationDate = DateTime.Now,
                Value = value.SubstringM(0, 1024),
                Code = code.SubstringM(0, 64)
            });

            if (buffers.Count > MaxBufferSize)
            {
                StackOverFlow = true;
                buffers.Clear();
            }

        }
        #endregion
    }
}
