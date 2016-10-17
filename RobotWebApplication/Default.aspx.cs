using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RobotWebApplication
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(DateTime.UtcNow.AddHours(3.5));
            foreach (var item in Tazeyab.Common.EventsLog.GeneralLogs.getNewLogsAsHTML())
            {
                Response.Write(item.Replace("[", "<").Replace("]", ">") + "<br />");
            }
        }
    }
}