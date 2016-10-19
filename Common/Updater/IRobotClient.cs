using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mn.NewsCms.Common
{
    public enum CommandList
    {
        StartUpdater,
        UpdateNewsPaper,
        SetIcon,
        NoneStopUpdater,
        UpdaterByTimer,
    }
    public interface IRobotClient<T>
    {
        T EndPoint { get; set; }
        List<string> GetNewLogs();
        string Execution(CommandList command);
        void PokeMe();
        void Updater(string DurationCode);
    }

}
