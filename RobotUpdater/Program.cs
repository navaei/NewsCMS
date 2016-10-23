using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Robot.Updater;

namespace Mn.NewsCms.UpdaterApp
{
    class Program
    {
        private static IUnityContainer container;
        static void Main(string[] args)
        {
            container = Bootstrapper.Initialise();
            if (args == null || !args.Any() || args[0].Contains("updater"))
            {
                new FeedUpdater(container.Resolve<IAppConfigBiz>(), container.Resolve<IFeedBusiness>(), container.Resolve<IFeedItemBusiness>(), container.Resolve<IUpdaterDurationBusiness>()).AutoUpdater();
                GeneralLogs.WriteLogInDB("Bye.... ", TypeOfLog.End, typeof(FeedUpdater));
            }
            else if (args[0].ToLower().Contains("feedschecker"))
            {
                GeneralLogs.WriteLogInDB("Starttrr Check Feed ", TypeOfLog.Start, "FeedHealth");
                var bads = FeedCheck();
                GeneralLogs.WriteLogInDB("Number of bad feeds " + bads, TypeOfLog.End, "FeedHealth");
            }

        }
        static int FeedCheck()
        {
            var feeds = container.Resolve<IFeedBusiness>().GetList().Where(f => f.Deleted == DeleteStatus.Temporary && f.UpdatingErrorCount < 7).ToList();
            var bads = 0;
            foreach (var feed in feeds)
            {
                try
                {
                    if (new FeedUpdater(container.Resolve<IAppConfigBiz>(), container.Resolve<IFeedBusiness>(), container.Resolve<IFeedItemBusiness>(), container.Resolve<IUpdaterDurationBusiness>()).UpdatingFeed(feed, false) > 0)
                    {
                        feed.Deleted = DeleteStatus.Active;
                        feed.UpdatingErrorCount = 0;
                    }
                    else
                    {
                        feed.UpdatingErrorCount = (byte)(feed.UpdatingErrorCount + 1);
                        bads++;
                    }
                }
                catch (Exception ex)
                {
                    feed.UpdatingErrorCount = (byte)(feed.UpdatingErrorCount + 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("bad feed: " + feed.Link + " " + ex.Message.SubstringM(0, 80));
                    bads++;
                }

                container.Resolve<IFeedBusiness>().Edit(feed);
            }
            return bads;
        }
    }
}
