using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tazeyab.Common.EventsLog;
using Tazeyab.Common.Models;
using Tazeyab.Common;
using Tazeyab.Common.Share;
using Mn.Framework.Common;

namespace Tazeyab.Common.Updater
{
    public abstract class BaseUpdaterServer<EndPointType> : BaseUpdater
    {
        #region Properties
        TazehaContext context;
        public List<IRobotClient<EndPointType>> RobotClients = new List<IRobotClient<EndPointType>>();
        public override bool StopUpdater
        {
            get
            {
                if (getIsRun().HasFlag(UpdaterList.UpdaterServer))
                    return false;
                return true;

            }
            set
            {
                setIsRun(UpdaterList.UpdaterServer, !value);
            }
        }

        #endregion

        public BaseUpdaterServer(bool? IsLocaly)
            : base(IsLocaly)
        { }

        public override void UpdateIsParting()
        {
            try
            {
                var Context = new TazehaContext();
                StartUp inputparams = new StartUp();
                EventsLog.GeneralLogs.WriteLogInDB(">Start Updater As service", TypeOfLog.Start);
                #region ForFirst
                List<UpdateDuration> periods = new List<UpdateDuration>();
                if (DurationDic.Count == 0)
                {
                    if (IsLocaly == true)
                        periods = ServiceFactory.Get<IUpdaterDurationBusiness>().GetList().Where(x => x.IsParting.Value == true && x.IsLocalyUpdate == true).ToList();
                    else
                        periods = ServiceFactory.Get<IUpdaterDurationBusiness>().GetList().Where(x => x.IsParting.Value == true).ToList();//static List<FeedContract> FeedsCandidate = new List<FeedContract>();

                    foreach (var period in periods)
                        DurationDic.Add(period, 0);
                }
                else
                {
                    periods = DurationDic.Select(x => x.Key).ToList();
                }
                #endregion
                int durationCounter = 0;
                foreach (var duration in periods)
                {
                    //if (StopUpdater)
                    //    return;
                    inputparams.StartUpConfig = duration.Code;
                    var FeedsCount = Context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id
                        && x.Site.IsBlog == inputparams.IsBlog
                        && ((int)x.Deleted < 1 || (int)x.Deleted > 10 || x.Deleted == null)).Count();

                    TimeSpan delaytime = TimeSpan.Parse(duration.DelayTime);
                    var Partnumber = delaytime.Hours * 60 / Config.GetTimeInterval();//20 min intervall
                    //var TopCount = (duration.FeedsCount / Partnumber) != 0 ? (duration.FeedsCount / Partnumber) : (duration.FeedsCount % Partnumber);
                    var TopCount = (FeedsCount / Partnumber) != 0 ? (FeedsCount / Partnumber) : (FeedsCount % Partnumber);
                    if (TopCount == 0)
                        continue;
                    inputparams.TopCount = TopCount + 1;

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

                    if (!StartByDuration(inputparams, duration, durationCounter++))
                        continue;

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
                EventsLog.GeneralLogs.WriteLogInDB(">End UpdateIsParting as Service", TypeOfLog.Info);
            }
            catch (Exception ex)
            {
                EventsLog.GeneralLogs.WriteLogInDB("Errror UpdateIsParting : " + ex.Message, TypeOfLog.Error);
                StopUpdater = true;
                DurationDic.Clear();
            }
        }
        public override bool StartByDuration(StartUp inputParams, UpdateDuration duration, int counter)
        {
            try
            {
                var context = new TazehaContext();
                GeneralLogs.WriteLogInDB("BaseUpdaterServer StartUptare PriorityCode:" + inputParams.StartUpConfig + " StartIndex:" + inputParams.StartIndex);
                IEnumerable<Feed> arr = null;
                //IEnumerable<Feed> arrLocaly = null;
                if (duration == null)
                    duration = context.UpdateDurations.FromHttpCache<UpdateDuration>().SingleOrDefault(x => x.Code == inputParams.StartUpConfig);
                if (duration.IsLocalyUpdate == true && IsLocaly == true)
                {
                    arr = context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id &&
                        //(x.Site.IsBlog == inputParams.IsBlog || inputParams.IsBlog == 2) &&
                        (x.Deleted == 0 || (int)x.Deleted > 10)).OrderBy(x => x.Id)
                        .Skip(inputParams.StartIndex)
                        .Take<Feed>(inputParams.TopCount).ToList();
                }
                else
                {
                    if (counter == 0 && FeedsCandidate.Count > Config.GetConfig<int>("MaxFeedCountAsService") * 2)
                        return false;

                    arr = context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id &&
                        x.Site.IsBlog == inputParams.IsBlog &&
                        (x.Deleted == 0 || (int)x.Deleted > 10)).OrderBy(x => x.Id).Skip(inputParams.StartIndex).Take<Feed>(inputParams.TopCount).ToList();
                }



                #region for sync client & server
                //in shart baraye inke server o updater ba ham hamahang shavand,va list alaki tond tond por nashavad
                //int sleepConter = 0;
                //while ((FeedsCandidate.Count > Config.getConfig<int>("MaxFeedCountAsService") * 2 ||
                //    (FeedsCandidateLocaly.Count > Config.getConfig<int>("MaxFeedCountAsService") * 2)) &&
                //    arr.Count() > Config.getConfig<int>("MaxFeedCountAsService") * 2)
                //{
                //    if (++sleepConter > 5)
                //    {
                //        PokeClients();
                //        sleepConter = 0;
                //    }
                //    System.Threading.Thread.Sleep(1000 * 60 * 2);//2 min;      
                //    GeneralLogs.WriteLog("Thread.Sleep 2 min(Feed Buffer OverFlow) [AsService]", TypeOfLog.Info);
                //}
                #endregion

                if (arr.Count() > 0 && duration.IsLocalyUpdate == true && IsLocaly == true)
                {
                    FeedsCandidateLocaly.AddRange(arr.ToList().ConvertToFeedContract());
                }
                else if (arr.Count() > 0)
                {
                    FeedsCandidate.AddRange(arr.ToList().ConvertToFeedContract());
                }

                GeneralLogs.WriteLog(string.Format("Add {0} Feed to Buffer.", arr.Count()), TypeOfLog.Info);
                //if (FeedsCandidate.Count > Config.getConfig<int>("MaxFeedCountAsService") ||
                //    FeedsCandidateLocaly.Count > Config.getConfig<int>("MaxFeedCountAsService"))
                //    PokeClients();
                //System.Threading.Thread.Sleep(1000 * 60);
                #region AfterFor
                //if (!duration.IsParting.Value)
                //    AfterOneCall(inputParams, duration);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLogInDB("StartByDuration " + ex.Message, TypeOfLog.Error);
                return false;
            }
        }

        public static List<FeedContract> getCandidateFeeds(int MaxSize, bool? IsLocaly)
        {
            if (IsLocaly == true)
                lock (FeedsCandidateLocaly)
                {
                    var ret = FeedsCandidateLocaly.Take(MaxSize).ToArray();
                    if (FeedsCandidateLocaly.Count > MaxSize)
                        FeedsCandidateLocaly.RemoveRange(0, MaxSize);
                    else if (FeedsCandidateLocaly.Count > 0)
                        FeedsCandidateLocaly.Clear();
                    return ret.ToList();
                }
            else
            {
                lock (FeedsCandidate)
                {
                    var ret = FeedsCandidate.Take(MaxSize).ToArray();
                    if (FeedsCandidate.Count > MaxSize)
                        FeedsCandidate.RemoveRange(0, MaxSize);
                    else if (FeedsCandidate.Count > 0)
                        FeedsCandidate.Clear();
                    return ret.ToList();
                }
            }
        }

        private void AfterOneCall(StartUp inputParams, UpdateDuration duration)
        {
            #region NoIsParting
            TazehaContext context = new TazehaContext();
            int ItemCountPriorityCode = context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id && x.Site.IsBlog == inputParams.IsBlog && (x.Deleted == 0 || (int)x.Deleted > 10)).Count();
            int LastCount = context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id && x.Site.IsBlog == inputParams.IsBlog && (x.Deleted == 0 || (int)x.Deleted > 10)).OrderBy(x => x.Id).Skip(inputParams.StartIndex).Take<Feed>(inputParams.TopCount).Count();
            int NextCount = context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id && x.Site.IsBlog == inputParams.IsBlog && (x.Deleted == 0 || (int)x.Deleted > 10)).OrderBy(x => x.Id).Skip(inputParams.StartIndex + inputParams.TopCount).Take<Feed>(inputParams.TopCount).Count();
            if (NextCount > 0)
            {

                var LastUpdateIndex = context.ProjectSetups.SingleOrDefault(x => x.Title == "LastUpdat:" + inputParams.StartUpConfig);
                if (LastUpdateIndex != null)
                {
                    //LastUpdateIndex.Value = (InputParams.StartIndex + InputParams.TopCount + InputParams.TopCount).ToString();
                    LastUpdateIndex.Value = (inputParams.StartIndex + inputParams.TopCount).ToString();
                    context.SaveChanges();
                }
                else
                {
                    //entiti.ProjectSetup.AddObject(new ProjectSetup { Title = "LastUpdat:" + InputParams.StartUpConfig, Value = (InputParams.StartIndex + InputParams.TopCount + InputParams.TopCount).ToString(), Meaning = "last index updater of feed of priority" });
                    context.ProjectSetups.Add(new ProjectSetup { Title = "LastUpdat:" + inputParams.StartUpConfig, Value = (inputParams.StartIndex + inputParams.TopCount).ToString(), Meaning = "last index updater of feed of priority" });
                    context.SaveChanges();
                }
                inputParams.StartIndex += inputParams.TopCount;
                if (!duration.IsParting.HasValue || !duration.IsParting.Value)
                {
                    //sleep
                    #region sleep
                    PokeClients();

                    while (FeedsCandidate.Count > Config.GetConfig<int>("MaxFeedCountAsService") ||
                        FeedsCandidateLocaly.Count > Config.GetConfig<int>("MaxFeedCountAsService"))
                        System.Threading.Thread.Sleep(1000 * 60);

                    #endregion
                    StartByDuration(inputParams, null, 0);
                }
            }
            else
            {
                //-----------------------When all items updated------------------
                if (duration != null)
                {
                    var LastUpdateIndex = context.ProjectSetups.SingleOrDefault(x => x.Title == "LastUpdat:" + inputParams.StartUpConfig);
                    if (LastUpdateIndex != null)
                    {
                        LastUpdateIndex.Value = "0";
                        context.SaveChanges();
                    }
                    else
                    {
                        context.ProjectSetups.Add(new ProjectSetup { Title = "LastUpdat:" + inputParams.StartUpConfig, Value = "0", Meaning = "last index updater of feed of priority" });
                        context.SaveChanges();
                    }
                    GeneralLogs.WriteLogInDB(">OK UpdaterSleeping... duration:" + duration.Code);
                    ///for test task with windows----                                           
                }
            }
            #endregion
        }

        protected void PokeClients()
        {
            foreach (var client in RobotClients)
                client.PokeMe();
        }

    }
}
