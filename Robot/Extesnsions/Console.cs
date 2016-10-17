using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tazeyab.Common.EventsLog;
using CLRConsole = System.Console;

namespace CrawlerEngine.Extesnsions
{
    public static class Console
    {

        public static void WriteLine(string str)
        {
            GeneralLogs.WriteLog(str);
        }

    }
}
