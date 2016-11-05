using System;
using System.Web.Mvc;
using System.Web.Routing;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.EventsLog;
//using System.Web.Http.Common;
using System.Web.Optimization;
using Mn.NewsCms.DomainClasses.UpdaterBusiness;
using Mn.NewsCms.Web.WebLogic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using CaptchaMvc.Infrastructure;
using Hangfire;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Robot.Updater;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;


// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
// visit http://go.microsoft.com/?LinkId=9394801

namespace Mn.NewsCms.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new Mn.NewsCms.Web.WebLogic.WebToolkit.MandatoryWww());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(RequiredMnAttribute));

            //ModelBinders.Binders.Add(typeof(DateTime), new PersianDateModelBinder());
            //ModelBinders.Binders.Add(typeof(DateTime?), new PersianDateModelBinder());

            Bootstrapper.Initialise();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Mn.GetMedia.AppConfig.SoundcloudApiKeys = new List<string>() { ConfigurationManager.AppSettings["SoundcloudApiKey"] };
            //Mn.GetMedia.AppConfig.ApiKey = ConfigurationManager.AppSettings["TelegramGetMediaApiKey"];

            //ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder("yyyy-mm-dd hh:mm"));
            //ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder("yyyy-mm-dd hh:mm"));

            //DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("mobile")
            //{
            //    ContextCondition = (context => context.Request.IsMobile())
            //});
            CaptchaUtils.CaptchaManager.StorageProvider = new CookieStorageProvider();
            //Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLogInDB("Application Start at " + DateTime.Now.NowHour(), TypeOfLog.Start);  
            BackgroundJob.Schedule(() => Updater(), TimeSpan.FromMinutes(15));
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //    Exception ex = Server.GetLastError();
            //    try
            //    {
            //        if (!Request.UserAgent.ContainsX("bot"))
            //            Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLogInDB(string.Format("Application_Error {0}\n {1}\n {2}\n", Request.Url.ToString().SubstringM(0, 200), ex.Message.SubstringM(0, 500), Request.UserAgent.SubstringM(0, 100)), TypeOfLog.Error);
            //    }
            //    catch { }
            //    if (ex.Message.ToLower().Contains("file does not exist") || ex.Message.Contains("not found"))
            //        return;
            //    //else if (ex.Message.ToLower().Contains("Object reference not set to an instance of an object"))
            //    //    Response.Redirect("~/error/notfound");
            //    if (!string.IsNullOrEmpty(ex.Message))
            //        Response.Redirect(string.Format("~/error?aspxerrorpath={0}&msg={1}", Request.Url.ToString().SubstringM(0, 50), ex.Message.SubstringM(0, 100)));
            //    else
            //        Response.Redirect(string.Format("~/error?aspxerrorpath={0}&msg={1}", Request.Url.ToString().SubstringM(0, 50), string.Empty));
        }

        void Updater()
        {
            if (DateTime.Now.NowHour() > ServiceFactory.Get<IAppConfigBiz>().GetConfig<int>("StartNightly") &&
                DateTime.Now.NowHour() < ServiceFactory.Get<IAppConfigBiz>().GetConfig<int>("EndNightly"))
            {
                //-----Nightly-----
                LuceneBase.TodayItemsCount = 0;
            }
            else
            {
                new FeedUpdater(ServiceFactory.Get<IAppConfigBiz>(),
                    ServiceFactory.Get<IFeedBusiness>(),
                    ServiceFactory.Get<IFeedItemBusiness>(),
                    ServiceFactory.Get<IUpdaterDurationBusiness>()).AutoUpdater();
            }
        }

        protected void Application_End()
        {
            try
            {
                LuceneBase _LuceneRepository = new LuceneSaverRepository();
                _LuceneRepository.Optimize();
                GeneralLogs.WriteLogInDB(">INFO application ending" + DateTime.Now.ToString("yyyMMddHH"));

            }
            catch { }
        }
        void Session_Start(object sender, EventArgs e)
        {
        }
        void Session_End(object sender, EventArgs e)
        {
        }
        void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        void Application_EndRequest(object sender, EventArgs e)
        {

        }
        //public override string GetVaryByCustomString(HttpContext context, string custom)
        //{
        //    try
        //    {
        //        if (custom.ToLower() == "ismobile" && context.Request.IsMobile())
        //            return "mobile";

        //    }
        //    catch { }
        //    return base.GetVaryByCustomString(context, custom);
        //}        
    }
}
