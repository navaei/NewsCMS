using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Updater;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Config;
using Mn.Framework.Common;

namespace Mn.NewsCms.DomainClasses.UpdaterBusiness
{
    public class RobotClient<EndPointType> : Mn.NewsCms.Common.IRobotClient<EndPointType>
    {
        //public RobotClient(string endPointAddress)
        //{
        //    EndPointAddress = endPointAddress;
        //}
        //public RobotClient(IBaseService baseService)
        //{
        //    BaseServiceProvider = baseService;
        //}
        EndPointType endpoint;
        public List<string> GetNewLogs()
        {
            throw new NotImplementedException();
        }

        public string Execution(CommandList command)
        {
            try
            {
                if (command == CommandList.StartUpdater)
                {
                    //string remoteUpdater = Config.getConfig<string>("RemoteUpdater");
                    if (EndPoint.GetType() == typeof(string))
                    {
                        var remoteUpdater = EndPoint as string;
                        GeneralLogs.WriteLog("Poke Client " + remoteUpdater, TypeOfLog.Start);
                        WebClient client = new WebClient();
                        client.DownloadData(remoteUpdater + "Execution?command=" + command.ToString());
                        GeneralLogs.WriteLog("Poke Client", TypeOfLog.OK);
                        return "Start Updater";
                    }
                    else if (EndPoint.GetType().IsClass)
                    {
                        (EndPoint as BaseUpdaterClient).Poke();
                    }
                }
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog(ex.Message,TypeOfLog.Error);
            }
            return string.Empty;
        }

        public void PokeMe()
        {
            new Task(() => { Execution(CommandList.StartUpdater); }).Start();
            //Execution(CommandList.StartUpdater);
        }

        public EndPointType EndPoint
        {
            get { return endpoint; }
            set { endpoint = value; }
        }

        public static string DefaultDistributeHostUrl
        {
            get
            {
                return ServiceFactory.Get<IAppConfigBiz>().GetConfig<string>("RemoteUpdater");
            }
        }

        public void Updater(string DurationCode)
        {
            throw new NotImplementedException();
        }
    }
}