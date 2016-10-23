using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Updater;
using Mn.NewsCms.Robot;
using Mn.NewsCms.Robot.Updater;
using Mn.NewsCms.DomainClasses.UpdaterBusiness;

namespace Mn.NewsCms.Web.WebLogic
{
    public static class AppUpdater
    {
        static IBaseServer baseservice;
        public static void RunClassicUpdater()
        {

        }
        public static void RunUpdaterServer()
        {
            #region RemoteUpdater
            var remoteUpdater = RobotClient<string>.DefaultDistributeHostUrl;
            if (!string.IsNullOrEmpty(remoteUpdater) && !string.IsNullOrEmpty(Utility.GetWebText(remoteUpdater[remoteUpdater.Length - 1] == '/' ? remoteUpdater.Remove(remoteUpdater.Length - 1) : remoteUpdater)))
            {
                GeneralLogs.WriteLogInDB("GetWebText from " + remoteUpdater, TypeOfLog.Info);
                IRobotClient<string> client = new RobotClient<string>() { EndPoint = remoteUpdater };
                var server = new UpdaterServer<string>(client, null);
                server.UpdateIsParting();
            }
            else
                GeneralLogs.WriteLogInDB("Can not GetWebText from " + remoteUpdater, TypeOfLog.Error);
            #endregion
        }
        public static void RunServerWithClientUpdater(IAppConfigBiz appConfigBiz, IFeedBusiness feedBusiness, IFeedItemBusiness feedItemBusiness, IUpdaterDurationBusiness updaterDurationBusiness)
        {
            Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLogInDB("RunServerWithClientUpdater", TypeOfLog.Start);
            var baseserver = new BaseServer(appConfigBiz, feedBusiness, feedItemBusiness, updaterDurationBusiness);
            var Clientupdater = new ClientUpdater(baseserver, feedBusiness, appConfigBiz, true);
            IRobotClient<BaseUpdaterClient> client = new RobotClient<BaseUpdaterClient>() { EndPoint = Clientupdater };
            var server = new UpdaterServer<BaseUpdaterClient>(client, true);
            server.UpdateIsParting();
            Clientupdater.AutoUpdateFromServer();
        }
        public static void Optimize()
        {
            baseservice.Optimize();
        }
    }
}