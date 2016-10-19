using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Share;
using Mn.Framework.Common;
using Mn.NewsCms.Common.Config;

namespace Mn.NewsCms.Common.Updater
{
    public abstract class BaseUpdater
    {
        #region Fields
        protected TazehaContext Context;
        public abstract bool StopUpdater { get; set; }
        private bool? IsPause;
        protected bool? IsLocaly;
        private DateTime? LastHour;
        private delegate void StartUpdater();
        private int StartOfEndDate { get { return 1; } }
        private int EndOfEndDate { get { return 6; } }
        protected static Dictionary<UpdateDuration, int> DurationDic = new Dictionary<UpdateDuration, int>();
        protected static List<FeedContract> FeedsCandidate = new List<FeedContract>();
        protected static List<FeedContract> FeedsCandidateLocaly = new List<FeedContract>();
        private IAppConfigBiz _config;
        public IAppConfigBiz Config
        {
            get
            {
                if (_config == null)
                    _config = ServiceFactory.Get<IAppConfigBiz>();
                return _config;
            }
        }
        #endregion

        #region PublicMethod

        public BaseUpdater(bool? Islocaly)
        {
            IsLocaly = Islocaly;
            Context = new TazehaContext();
        }
        public static UpdaterList UpdatersIsRun()
        {
            if (System.Web.HttpContext.Current.Application["RunUpdaterStatus"] != null)
            {
                return (UpdaterList)System.Web.HttpContext.Current.Application["RunUpdaterStatus"];
            }
            else
            {
                System.Web.HttpContext.Current.Application.Add("RunUpdaterStatus", UpdaterList.None);
                return UpdaterList.None;
            }

        }
        public UpdaterList getIsRun()
        {

            if (System.Web.HttpContext.Current.Application["RunUpdaterStatus"] != null)
                return (UpdaterList)System.Web.HttpContext.Current.Application["RunUpdaterStatus"];
            return UpdaterList.None;


        }
        public void setIsRun(UpdaterList value, bool state)
        {
            if (System.Web.HttpContext.Current.Application["RunUpdaterStatus"] == null)
                System.Web.HttpContext.Current.Application.Add("RunUpdaterStatus", UpdaterList.None);

            var status = (UpdaterList)System.Web.HttpContext.Current.Application["RunUpdaterStatus"];

            if (state)
            {
                System.Web.HttpContext.Current.Application["RunUpdaterStatus"] = status | value;
            }
            else
            {
                System.Web.HttpContext.Current.Application["RunUpdaterStatus"] = status ^ value;
            }

        }

        public void AutoUpdater()
        {
            StopUpdater = false;
            if (Config.GetConfig("DisableUpdater") == "1")
            {
                GeneralLogs.WriteLog("Updater is Disable", TypeOfLog.Info);
                return;
            }
            //------Starting after 10 min
            //System.Threading.Thread.Sleep(1000 * 60 * 10);           

            while (!StopUpdater)
            {
                DateTime datetime = DateTime.Now.AddHours(double.Parse(Config.GetConfig("Hour_UTC")));
                if (datetime.Hour >= StartOfEndDate && datetime.Hour < EndOfEndDate)
                {
                    StopUpdater = true;
                    return;
                }
                LastHour = datetime;
                UpdateIsParting();
                var now = DateTime.Now.AddHours(double.Parse(Config.GetConfig("Hour_UTC")));
                var delay = now.Subtract(datetime).Minutes;
                if (delay > 0 && delay < 20)
                {
                    IsPause = true;
                    System.Threading.Thread.Sleep(1000 * 60 * (Config.GetTimeInterval() - delay));//1 sec >> 1 min >> 10 min  
                    IsPause = false;
                }

            }
        }
        public void Start(StartUp inputParams)
        {
            StartByDuration(inputParams, new UpdateDuration(), 0);
        }
        public void Continue(StartUp inputparams)
        {
            int LastUpdateIndex = 0;
            var lastupdateindex = Config.GetConfig("LastUpdat:" + inputparams.StartUpConfig.Trim());
            if (!string.IsNullOrEmpty(lastupdateindex))
                LastUpdateIndex = int.Parse(lastupdateindex);
            inputparams.StartIndex = LastUpdateIndex;
            //-------set top count---------
            var duration = ServiceFactory.Get<IUpdaterDurationBusiness>().GetList().SingleOrDefault(x => x.Code == inputparams.StartUpConfig);
            if (duration.IsParting.HasValue && duration.IsParting.Value)
            {
                var AllFeed = ServiceFactory.Get<IFeedBusiness>().GetList().Where(x => x.UpdateDurationId.Value == duration.Id &&
                    x.Site.IsBlog == inputparams.IsBlog &&
                    (x.Deleted == 0 || (int)x.Deleted > 10)).Count();
                TimeSpan delaytime = TimeSpan.Parse(duration.DelayTime);
                var Partnumber = delaytime.Hours * 60 / 15;//15 min intervall
                var TopCount = AllFeed / Partnumber != 0 ? AllFeed / Partnumber : AllFeed % Partnumber;
                inputparams.TopCount = TopCount;
            }
            Start(inputparams);
        }

        public abstract bool StartByDuration(StartUp start, UpdateDuration duration, int counter);
        public abstract void UpdateIsParting();
        #endregion
    }
}
