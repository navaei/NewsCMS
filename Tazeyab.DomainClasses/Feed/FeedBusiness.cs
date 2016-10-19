using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Model;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;

namespace Mn.NewsCms.DomainClasses
{
    public class FeedBusiness : BaseBusiness<Feed>, IFeedBusiness
    {
        public FeedBusiness(IUnitOfWork dbContext) : base(dbContext)
        {
        }

        public override Feed Get(long feedId)
        {
            return base.GetList().SingleOrDefault(f => f.Id == feedId);
        }
        public Feed GetWithSite(long feedId)
        {
            return base.GetList().Include(f => f.Site).SingleOrDefault(f => f.Id == feedId);
        }

        public IQueryable<Feed> GetList()
        {
            return base.GetList().Include(f => f.UpdateDuration);
        }
        public OperationStatus CreateEdit(Feed feed)
        {
            if (feed.Id == 0)
            {
                feed.CreationDate = DateTime.Now;
                return base.Create(feed);
            }
            else
                return base.Update(feed);

        }
        public OperationStatus Edit(Feed feed)
        {
            return base.Update(feed);
        }
        public OperationStatus UpdateFeed(Feed feed)
        {
            var dbFeed = Get(feed.Id);

            dbFeed.Deleted = feed.Deleted;
            dbFeed.LastUpdaterVisit = feed.LastUpdaterVisit;
            dbFeed.LastUpdateDateTime = feed.LastUpdateDateTime;
            dbFeed.LastUpdatedItemUrl = feed.LastUpdatedItemUrl;
            dbFeed.UpdateDurationId = feed.UpdateDurationId;
            dbFeed.UpdateSpeed = feed.UpdateSpeed;
            dbFeed.UpdatingCount = feed.UpdatingCount;
            dbFeed.UpdatingErrorCount = feed.UpdatingErrorCount;

            return base.Update(dbFeed);
        }
        public int GetCount()
        {
            return base.GetList().Count();
        }
        public OperationStatus DisableTemporary(long feedId)
        {
            var feed = Get(feedId);
            feed.Deleted = DeleteStatus.Temporary;
            feed.UpdatingErrorCount = 1;
            return Edit(feed);
        }

        public void CheckForChangeDuration(Feed feed, bool hasNewFeedItem)
        {
            if (!feed.LastUpdateDateTime.HasValue)
            {
                feed.LastUpdateDateTime = DateTime.Now;
                return;
            }


            TimeSpan lastupdatetime = DateTime.Now.Subtract(feed.LastUpdateDateTime.Value);
            TimeSpan delaytime = TimeSpan.Parse(feed.UpdateDuration.DelayTime);

            //---agar dar baze zamani kamtar az delay tarif shode feed update shode bood
            if (hasNewFeedItem && lastupdatetime < delaytime.Add(new TimeSpan(0, 10, 0)))
            {
                SpeedUP(feed);
            }
            else if (!hasNewFeedItem && lastupdatetime > delaytime)
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Friday)
                    SpeedDOWN(feed);
            }

        }
        public void SpeedDOWN(Feed feed)
        {
            feed.LastUpdateDateTime = DateTime.Now;
            //-----feed hanooz update nashode ast------------
            if (feed.UpdateSpeed < -5)
            {
                UpdateDuration newduration = ServiceFactory.Get<IUpdaterDurationBusiness>().GetList().Where<UpdateDuration>(x => x.PriorityLevel > feed.UpdateDuration.PriorityLevel).OrderBy(x => x.PriorityLevel).First();
                feed.UpdateDurationId = newduration.Id;
                feed.UpdateSpeed = 4;
                Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLogInDB("Change Duration(-) of Feed:" + feed.Id + " Link:" + feed.Link + " NewDuration:" + newduration.Id);
            }
            else
                feed.UpdateSpeed = feed.UpdateSpeed - 1;
            Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLog("SpeedDown Feed:" + feed.Id + " Link:" + feed.Link, TypeOfLog.Info);
        }
        public void SpeedUP(Feed feed)
        {
            feed.LastUpdateDateTime = DateTime.Now;
            if (feed.UpdateSpeed > 5)
            {
                //------feed ro saritar pooyesh kon(TAghire level)----
                var newdurations = ServiceFactory.Get<IUpdaterDurationBusiness>().GetList().Where<UpdateDuration>(x => x.PriorityLevel < feed.UpdateDuration.PriorityLevel);
                if (newdurations.Count() == 0)
                {
                    //-------agar be kamtarin baze updater resid---1Hour-----
                    feed.UpdateSpeed = 0;
                }
                else
                {
                    UpdateDuration newduration = newdurations.OrderByDescending(x => x.PriorityLevel).First();
                    feed.UpdateDurationId = newduration.Id;
                    feed.UpdateSpeed = -4;
                    GeneralLogs.WriteLogInDB("Change Duration(+) of Feed:" + feed.Id + " Link:" + feed.Link + " NewDuration:" + newduration.Id);
                }
            }
            else
                feed.UpdateSpeed = feed.UpdateSpeed + 1;

            GeneralLogs.WriteLog("SpeedUp Feed:" + feed.Id + " Link:" + feed.Link, TypeOfLog.Info);
        }


        public OperationStatus CreateFeedLog(FeedLog feedLog)
        {
            feedLog.CreateDate = DateTime.Now;
            return base.Create<FeedLog>(feedLog);
        }

        public IQueryable<FeedLog> GetListLogs()
        {
            return base.GetList<FeedLog>();
        }
    }
}
