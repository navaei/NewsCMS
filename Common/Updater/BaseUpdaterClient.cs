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
    public abstract class BaseUpdaterClient : BaseUpdater
    {
        #region Properties
        TazehaContext context;
        public IBaseServer BaseServer { get; set; }
        private static Dictionary<UpdateDuration, int> DurationDic = new Dictionary<UpdateDuration, int>();
        public override bool StopUpdater
        {
            get
            {
                if (getIsRun().HasFlag(UpdaterList.UpdaterClient))
                    return false;
                return true;

            }
            set
            {
                setIsRun(UpdaterList.UpdaterClient, !value);
            }
        }
        //public override bool StopUpdater
        //{
        //    set
        //    {
        //        System.Web.HttpRuntime.Cache["StopUpdaterClient"] = value;
        //    }
        //    get
        //    {
        //        if (System.Web.HttpRuntime.Cache["StopUpdaterClient"] != null)
        //            return bool.Parse(System.Web.HttpRuntime.Cache["StopUpdaterClient"].ToString());
        //        else
        //            return true;
        //    }
        //}
        #endregion

        public BaseUpdaterClient(IBaseServer baseservice, bool? isLocaly)
            : base(isLocaly)
        {
            BaseServer = baseservice;
        }


        public override bool StartByDuration(Common.Models.StartUp start, UpdateDuration duration, int counter)
        {
            throw new NotImplementedException();
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
                    StartByDuration(inputParams, null, 0);
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

        public abstract void Poke();

    }
}
