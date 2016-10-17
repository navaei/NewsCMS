using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Services;
using System.Threading;
using RobotWebApplication.ServiceReferenceTazeyab;
using Tazeyab.Common.EventsLog;
using Tazeyab.CrawlerEngine.SiteImprovement;
using Tazeyab.CrawlerEngine.Updater;
using ACorns.WCF.DynamicClientProxy;
using Tazeyab.Common;
using System.Threading.Tasks;
namespace RobotWebApplication
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UpdWebService : System.Web.Services.WebService, Tazeyab.Common.IRobotClient<string>
    {
        [WebMethod]
        public List<string> GetNewLogs()
        {
            return GeneralLogs.getNewLogs();
        }

        [WebMethod]
        public string Execution(CommandList command)
        {

            if (command == CommandList.StartUpdater)
            {
                if (!(Tazeyab.Common.Updater.BaseUpdater.UpdatersIsRun() == Tazeyab.Common.Models.Enums.UpdaterList.UpdaterClient))
                {
                    Tazeyab.Common.IBaseServer baseservice = WCFClientProxy<Tazeyab.Common.IBaseServer>.GetReusableInstance("WSHttpBinding_IBaseServer");
                    ClientUpdater updater = new ClientUpdater(baseservice, null);
                    new Task(() => { updater.AutoUpdateFromServer(); }).Start();
                    Tazeyab.Common.EventsLog.GeneralLogs.WriteLog("Run  updater.AutoUpdateAsService", TypeOfLog.Start);
                    return "Start AutoUpdater";
                }
                else
                    Tazeyab.Common.EventsLog.GeneralLogs.WriteLog("StopUpdating==false(Updater is running...)", TypeOfLog.Info);

                return "Updater is running";
            }
            else if (command == CommandList.NoneStopUpdater)
            {
                Tazeyab.Common.IBaseServer baseservice = WCFClientProxy<Tazeyab.Common.IBaseServer>.GetReusableInstance("WSHttpBinding_IBaseServer");
                ClientUpdater updater = new ClientUpdater(baseservice, null);
                updater.NoneStopUpdateFromServer(Config.getConfig("DefaultDurationCode"));
            }
            else if (command == CommandList.UpdaterByTimer)
            {
                Tazeyab.Common.IBaseServer baseservice = WCFClientProxy<Tazeyab.Common.IBaseServer>.GetReusableInstance("WSHttpBinding_IBaseServer");
                ClientUpdater updater = new ClientUpdater(baseservice, null);
                updater.UpdateFromServerByTimer(Config.getConfig<string>("DefaultDurationCode"));
            }
            else if (command == CommandList.SetIcon)
            {
                Tazeyab.CrawlerEngine.SiteImprovement.SiteIcon.StartSetSiteIcons(0);
            }
            else if (command == CommandList.UpdateNewsPaper)
            {
                Tazeyab.CrawlerEngine.Updater.NewsPaperUpdater updater = new NewsPaperUpdater();
                updater.Start(new Tazeyab.Common.Models.StartUp() { StartUpConfig = "NewsPaper" });
                updater.UploadNewsPaperImgToFtp();
            }
            return string.Empty;
        }

        [WebMethod]
        public void Updater(string DurationCode)
        {
            Tazeyab.Common.IBaseServer baseservice = WCFClientProxy<Tazeyab.Common.IBaseServer>.GetReusableInstance("WSHttpBinding_IBaseServer");
            ClientUpdater updater = new ClientUpdater(baseservice, null);
            var updaterbyduration = new Task(() => { updater.UpdateFromServerByTimer(string.IsNullOrEmpty(DurationCode) ? Config.getConfig<string>("DefaultDurationCode") : DurationCode); });
            updaterbyduration.Start();
        }

        public string EndPoint
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void PokeMe()
        {
            throw new NotImplementedException();
        }

    }
}
