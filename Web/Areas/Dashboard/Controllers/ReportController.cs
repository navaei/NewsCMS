using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tazeyab.Web.Areas.Dashboard.Controllers
{
    public partial class ReportController : BaseAdminController
    {
        // GET: Dashboard/Report
        public virtual ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult SearchHistory()
        {
            return View();
        }
        public virtual ActionResult RemoteRequest()
        {
            return View();
        }
        public virtual ActionResult FeedLogs()
        {
            return View();
        }
        public virtual JsonResult SearchHistories_Read([DataSourceRequest] DataSourceRequest request)
        {
            var query = Ioc.SearchHistoryBiz.GetList();
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));
            }
            var model = query.Select(s => new
            {
                s.Id,
                s.CreationDate,
                s.SearchKey,
                s.UserId
            });
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult RemoteRequests_Read([DataSourceRequest] DataSourceRequest request)
        {
            var query = Ioc.BlogService.GetListRemoteRequest();
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));
            }
            var model = query.Select(rl => new
            {
                rl.Id,
                rl.Content,
                rl.Controller,
                rl.CreationDate,
                rl.RequestRefer,
            });
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult FeedLogs_Read([DataSourceRequest] DataSourceRequest request)
        {
            var query = Ioc.FeedBiz.GetListLogs();
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));
            }
            var model = query.Select(log => new
            {
                log.Id,
                log.FeedId,
                log.Feed.Title,
                log.ItemsCount,
                log.HasError,
                log.Message,
                log.InitDate,
                log.CreateDate,
            });
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}