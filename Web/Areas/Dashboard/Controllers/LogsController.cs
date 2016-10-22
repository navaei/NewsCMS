using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class LogsController : Controller
    {
        private readonly ILogsBusiness _logsBusiness;

        public LogsController(ILogsBusiness logsBusiness)
        {
            _logsBusiness = logsBusiness;
        }

        public virtual ActionResult Index(bool indb = true)
        {
            ViewBag.InDb = indb;
            return View();
        }
        public virtual JsonResult Logs_Read([DataSourceRequest] DataSourceRequest request, bool indb = true)
        {
            IQueryable<LogsBuffer> res;
            if (!request.Sorts.Any())
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));

            if (indb)
            {
                res = _logsBusiness.GetList();
                return Json(res.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(GeneralLogs.getLogs().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
        }
        public virtual ActionResult ClearCache(bool indb)
        {
            if (indb)
                _logsBusiness.DeleteAll();
            else
                GeneralLogs.ClearCache();

            return RedirectToAction(MVC.Dashboard.Logs.Index(indb));
        }
    }
}