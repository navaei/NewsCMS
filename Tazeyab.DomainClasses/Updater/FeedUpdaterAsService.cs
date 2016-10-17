using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.EventsLog;
using Tazeyab.Common.Models;
using Tazeyab.Common.Updater;


namespace Tazeyab.DomainClasses.UpdaterBusiness
{
    public class UpdaterServer<EndPointType> : BaseUpdaterServer<EndPointType>
    {
        public UpdaterServer(List<IRobotClient<EndPointType>> robotClients, bool? IsLocaly)
            : base(IsLocaly)
        {
            RobotClients = robotClients;
        }
        public UpdaterServer(IRobotClient<EndPointType> robotClient, bool? IsLocaly)
            : base(IsLocaly)
        {
            RobotClients.Add(robotClient);
        }
    }
}