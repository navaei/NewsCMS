using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class UpdaterController : Controller
    {
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly IFeedBusiness _feedBusiness;
        private readonly IFeedItemBusiness _feedItemBusiness;
        private readonly IUpdaterDurationBusiness _updaterDurationBusiness;

        public UpdaterController(IAppConfigBiz appConfigBiz, IFeedBusiness feedBusiness, IFeedItemBusiness feedItemBusiness, IUpdaterDurationBusiness updaterDurationBusiness)
        {
            _appConfigBiz = appConfigBiz;
            _feedBusiness = feedBusiness;
            _feedItemBusiness = feedItemBusiness;
            _updaterDurationBusiness = updaterDurationBusiness;
        }

        // GET: Updater
        public virtual ActionResult Index()
        {
            return View();
        }

        //[OutputCache(Duration = 100)]
        public virtual ActionResult Start(string pass)
        {
            if (pass != "777")
            {
                ViewBag.UpdaterStatus = "Password incorrect!";
                return View();
            }
            var subc = new TimeSpan(0, 10, 0);
            if (HttpContext.Application["LastStartDateTime"] == null || DateTime.Parse(HttpContext.Application["LastStartDateTime"].ToString()).AddMinutes(10) < DateTime.Now)
            {
                HttpContext.Application["LastStartDateTime"] = DateTime.Now;

                if (DateTime.Now.NowHour() > _appConfigBiz.GetConfig<int>("StartNightly") &&
               DateTime.Now.NowHour() < _appConfigBiz.GetConfig<int>("EndNightly"))
                {
                    //-----Nightly-----
                    LuceneBase.TodayItemsCount = 0;
                    ViewBag.UpdaterStatus = "Nightly";
                }
                else
                {
                    try
                    {
                        //-------Daily----
                        Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLog("-----Start Updater Ping----", TypeOfLog.Start);
                        var status = Mn.NewsCms.Common.Updater.BaseUpdater.UpdatersIsRun();
                        if (!status.HasFlag(UpdaterList.UpdaterClient))
                        {
                            Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLog("status != UpdaterList.UpdaterClient", TypeOfLog.Info);
                            AppUpdater.RunServerWithClientUpdater(_appConfigBiz, _feedBusiness, _feedItemBusiness, _updaterDurationBusiness);
                            ViewBag.UpdaterStatus = "Now Start";
                        }
                        else
                            ViewBag.UpdaterStatus = "Is Running...";


                    }
                    catch (Exception ex)
                    {
                        GeneralLogs.WriteLogInDB("UpdaterController " + ex.Message, TypeOfLog.Error);
                    }
                }
            }
            else
                ViewBag.UpdaterStatus = "Is Running";

            return View();
        }
        public virtual ActionResult Telegram()
        {
            return Redirect("/");
        }
    }
}