using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tazeyab.Common.EventsLog;
using Tazeyab.Common.Models;
using Tazeyab.Common;
using Tazeyab.Common.Share;

namespace Tazeyab.Common.Updater
{
    public abstract class UpdaterClassic : BaseUpdater
    {
        public override bool StopUpdater
        {
            get
            {
                if (getIsRun().HasFlag(UpdaterList.UpdaterClassic))
                    return false;
                return true;

            }
            set
            {
                setIsRun(UpdaterList.UpdaterClassic, !value);
            }
        }
        public override void UpdateIsParting()
        {
            //TazehaContext Context = new TazehaContext();
            StartUp inputparams = new StartUp();
            EventsLog.GeneralLogs.WriteLogInDB("Start Updater is parting localy", TypeOfLog.Start);
            #region ForFirst
            List<UpdateDuration> periods = new List<UpdateDuration>();
            if (DurationDic.Count == 0)
            {
                periods = Context.UpdateDurations.FromHttpCache<UpdateDuration>().Where(x => x.IsParting.HasValue && x.IsParting.Value == true && x.IsLocalyUpdate.HasValue && x.IsLocalyUpdate == true).ToList();
                foreach (var period in periods)
                {
                    //period.FeedsCount = entiti.Feeds.Where(x => x.Id.Value == period.Id).Count();
                    DurationDic.Add(period, 0);
                }
            }
            else
                periods = DurationDic.Select(x => x.Key).ToList();

            #endregion
            int durationCounter = 0;
            foreach (var duration in periods)
            {
                if (StopUpdater)
                    return;
                inputparams.StartUpConfig = duration.Code;
                var FeedsCount = Context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id && ((int)x.Deleted < 1 || (int)x.Deleted > 10)).Count();//&& (x.Site.IsBlog == inputparams.IsBlog || inputparams.IsBlog == 2) &&
                TimeSpan delaytime = TimeSpan.Parse(duration.DelayTime);
                var Partnumber = delaytime.Hours * 60 / Config.GetTimeInterval();//20 min intervall
                var TopCount = FeedsCount / Partnumber != 0 ? FeedsCount / Partnumber : FeedsCount % Partnumber;
                if (TopCount == 0)
                    continue;
                inputparams.TopCount = TopCount;

                #region get Last Index
                if (DurationDic[duration] == 0)
                {
                    int LastUpdateIndex = 0;
                    var lastupdateindex = Context.ProjectSetups.SingleOrDefault(x => x.Title == "LastUpdat:" + inputparams.StartUpConfig.Trim());
                    if (lastupdateindex != null)
                        LastUpdateIndex = int.Parse(lastupdateindex.Value);
                    DurationDic[duration] = LastUpdateIndex;
                }
                #endregion
                inputparams.StartIndex = DurationDic[duration];

                StartByDuration(inputparams, duration, durationCounter);

                #region save last state
                Context = new TazehaContext();
                var projStup = Context.ProjectSetups.SingleOrDefault(x => x.Title == "LastUpdat:" + inputparams.StartUpConfig);
                if (DurationDic[duration] + 1 >= FeedsCount)
                {
                    projStup.Value = "0";
                    Context.SaveChanges();
                    DurationDic[duration] = 0;
                }
                else
                {
                    DurationDic[duration] = DurationDic[duration] + TopCount;
                    projStup = Context.ProjectSetups.SingleOrDefault(x => x.Title == "LastUpdat:" + inputparams.StartUpConfig);
                    projStup.Value = (DurationDic[duration]).ToString();
                    Context.SaveChanges();
                }
                #endregion

            }
            EventsLog.GeneralLogs.WriteLogInDB("End UpdateIsParting", TypeOfLog.End, typeof(UpdaterClassic));
        }
        public UpdaterClassic(bool? IsLocaly)
            : base(IsLocaly)
        { }
        public override bool StartByDuration(StartUp start, UpdateDuration duration, int counter)
        {
            throw new NotImplementedException();
        }
    }
}
