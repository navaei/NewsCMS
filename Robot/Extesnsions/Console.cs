using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mn.NewsCms.Common.EventsLog;
using CLRConsole = System.Console;

namespace Mn.NewsCms.Robot.Extesnsions
{
    public static class Console
    {

        public static void WriteLine(string str)
        {
            GeneralLogs.WriteLog(str);
        }

    }
}
