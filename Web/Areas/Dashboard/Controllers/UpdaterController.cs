using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Robot.Updater;
using Mn.NewsCms.Web.WebLogic;
using Mn.Framework.Web.Model;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class UpdaterController : BaseAdminController
    {
        private readonly IFeedBusiness _feedBusiness;
        private readonly IUpdaterDurationBusiness _updaterDurationBusiness;

        public UpdaterController(IFeedBusiness feedBusiness, IUpdaterDurationBusiness updaterDurationBusiness)
        {
            _feedBusiness = feedBusiness;
            _updaterDurationBusiness = updaterDurationBusiness;
        }

        public virtual ActionResult Index()
        {
            ViewBag.Message = "";
            return View();
        }
        public virtual ActionResult Duration()
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Mn.NewsCms.Common.Resources.General.Edit, "/Dashboard/Updater/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Delete, "deleteGridRow('/Dashboard/Updater/Delete/#=Id#')"));
            return View(model);
        }
        public virtual ActionResult UpdateFeed(int feedId)
        {
            //var feed = _feedBusiness.Get(2);
            //feed.Deleted = Common.Share.FeedDeleteStatus.Active;
            //_feedBusiness.Edit(feed);
            var baseserver = new BaseServer();
            var feeds = new List<FeedContract>();
            var feed = _feedBusiness.GetWithSite(feedId);
            var feedcontract = feed.ToViewModel<FeedContract>();
            feedcontract.SiteTitle = feed.Site.SiteTitle;
            feedcontract.SiteUrl = feed.Site.SiteUrl;
            feeds.Add(feedcontract);
            (new ClientUpdater(baseserver, true)).FeedsUpdat(feeds);
            ViewBag.Message = "Feed Updated";
            return View("Index");
        }
        public virtual ActionResult Start(string param)
        {
            var status = Mn.NewsCms.Common.Updater.BaseUpdater.UpdatersIsRun();
            if (!status.HasFlag(UpdaterList.UpdaterClient))
            {
                Mn.NewsCms.Common.EventsLog.GeneralLogs.WriteLog("status != UpdaterList.UpdaterClient", TypeOfLog.Info);
                AppUpdater.RunServerWithClientUpdater();
            }
            //AppUpdater.RunServerWithClientUpdater();
            return View();
        }
        public virtual JsonResult Optimize()
        {
            AppUpdater.Optimize();
            return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public virtual ActionResult FeedsCheck()
        {
            var badFeeds = new List<FeedContract>();
            var baseserver = new BaseServer();
            var feeds = _feedBusiness.GetList().Where(f => f.Deleted == DeleteStatus.Temporary && f.UpdatingErrorCount < 7).ToList();
            ViewBag.OldCount = feeds.Count;
            foreach (var feed in feeds)
            {
                FeedContract feedContract = new FeedContract();
                try
                {
                    feedContract = feed.ToViewModel<FeedContract>();
                    feedContract = (new ClientUpdater(baseserver, true)).FeedUpdateAsService(feedContract, new List<string>());
                    if (!feedContract.FeedItems.Any())
                        badFeeds.Add(feedContract);

                    feed.Deleted = DeleteStatus.Active;
                    feed.UpdatingErrorCount = 0;
                }
                catch (Exception ex)
                {
                    feed.UpdatingErrorCount = (byte)(feed.UpdatingErrorCount + 1);
                    feedContract.SiteTitle = ex.Message.SubstringETC(0, 200) + (ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message.SubstringETC(0, 200) : string.Empty);
                    badFeeds.Add(feedContract);
                }
                _feedBusiness.Edit(feed);
            }
            ViewBag.NewCount = feeds.Count - badFeeds.Count;
            return View(badFeeds);
        }
        public virtual JsonResult Durations_Read([DataSourceRequest] DataSourceRequest request)
        {
            var query = _updaterDurationBusiness.GetList();
            var model = query.Select(ud => new
            {
                ud.Id,
                ud.Code,
                ud.DelayTime,
                ud.EnabledForUpdate,
                ud.EndSleepTimeHour,
                ud.FeedsCount,
                ud.IsDefault,
                ud.IsLocalyUpdate,
                ud.IsParting,
                ud.MaxSleepTime,
                ud.PriorityLevel,
                ud.ServiceLink,
                ud.StartIndex,
                ud.StartSleepTimeHour,
                ud.Title,
            });
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}