using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Tazeyab.Common;

namespace RobotWebApplication
{
    public class Global : System.Web.HttpApplication
    {
        static int FeedUpdatedCount;
        System.Timers.Timer timer = null;
        protected void Application_Start(object sender, EventArgs e)
        {
            //timer = new System.Timers.Timer(1000 * 60 * Config.getConfig<int>("TimeInterval"));
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();           
        }
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.UtcNow.AddHours(Config.getConfig<double>("UTCDelay")).Hour > Config.getConfig<int>("StartNightly") &&
                DateTime.UtcNow.AddHours(Config.getConfig<double>("UTCDelay")).Hour < Config.getConfig<int>("EndNightly"))
            {
                //--Nightly----
                //timer.Stop();
                FeedUpdatedCount = 0;
            }
            else
            {
                //--Daily----  
                //if (FeedUpdatedCount < Config.getConfig<int>("FeedCountInDuration"))
                //{
                UpdWebService upd = new UpdWebService();
                upd.Execution(Tazeyab.Common.CommandList.UpdaterByTimer);
                FeedUpdatedCount += Config.getConfig<int>("MaxFeedCountAsService");
                //}
                //else
                //    timer.Stop();
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}