using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

                if (DateTime.Now.NowHour() > ServiceFactory.Get<IAppConfigBiz>().GetConfig<int>("StartNightly") &&
               DateTime.Now.NowHour() < ServiceFactory.Get<IAppConfigBiz>().GetConfig<int>("EndNightly"))
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
                            AppUpdater.RunServerWithClientUpdater();
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