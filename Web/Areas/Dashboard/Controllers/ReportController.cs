using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.ExternalService;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class ReportController : BaseAdminController
    {
        private readonly ISearchHistoryBusiness _searchHistoryBusiness;
        private readonly IFeedBusiness _feedBusiness;
        private readonly IBlogService _blogService;

        public ReportController(ISearchHistoryBusiness searchHistoryBusiness, IFeedBusiness feedBusiness, IBlogService blogService)
        {
            _searchHistoryBusiness = searchHistoryBusiness;
            _feedBusiness = feedBusiness;
            _blogService = blogService;
        }

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
            var query = _searchHistoryBusiness.GetList();
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
            var query = _blogService.GetListRemoteRequest();
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
            var query = _feedBusiness.GetListLogs();
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