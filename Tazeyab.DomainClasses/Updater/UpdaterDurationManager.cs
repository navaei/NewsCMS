using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses
{
    public class UpdaterDurationBusiness : BaseBusiness<UpdateDuration>, IUpdaterDurationBusiness
    {
        public UpdaterDurationBusiness(IUnitOfWork dbContext) : base(dbContext)
        {
        }

        static List<UpdateDuration> durationList;
        public List<UpdateDuration> GetList()
        {
            if (durationList == null || !durationList.Any())
                durationList = base.GetList().ToList();
            return durationList;
        }
        public void PokeClients()
        {
            TazehaContext context = new TazehaContext();
            var nowUTCHour = DateTime.Now.NowHour();
            var durations = context.UpdateDurations.Where(x => x.IsLocalyUpdate.Value == false &&
                x.EnabledForUpdate == true &&
                !(nowUTCHour > x.StartSleepTimeHour &&
                nowUTCHour < x.EndSleepTimeHour));
            foreach (var duration in durations)
            {
                var ServiceLink = duration.ServiceLink;
                if (!string.IsNullOrEmpty(ServiceLink))
                {
                    GeneralLogs.WriteLog("Poke Client " + ServiceLink, TypeOfLog.Start);
                    WebClient client = new WebClient();
                    client.DownloadDataAsync(new Uri(ServiceLink + "Updater?DurationCode=" + duration.Code.Trim()));
                    GeneralLogs.WriteLog("Poke Client", TypeOfLog.OK);
                }
                System.Threading.Thread.Sleep(1000 * 60 * 2);
            }
        }
        public UpdateDuration GetLast(string Code, int CountOfFeed)
        {
            TazehaContext context = new TazehaContext();
            var duration = context.UpdateDurations.SingleOrDefault(x => x.Code.StartsWith(Code));
            if (duration.StartIndex > duration.FeedsCount)
                duration.StartIndex = 0;
            else
                duration.StartIndex = duration.StartIndex + CountOfFeed;
            context.SaveChanges();
            return duration;
        }

        public OperationStatus Edit(UpdateDuration duration)
        {
            return base.Update(duration);
        }
        public OperationStatus Edit(int id, int feedsCount, int startIndex)
        {
            var duration = Get(id);
            duration.FeedsCount = feedsCount;
            duration.StartIndex = startIndex;
            return base.Update(duration);
        }
    }
}