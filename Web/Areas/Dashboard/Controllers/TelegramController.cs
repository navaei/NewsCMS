using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.Framework.Web.Model;

namespace Tazeyab.Web.Areas.Dashboard.Controllers
{
    public partial class TelegramController : BaseAdminController
    {
        // GET: Dashboard/Telegram
        public virtual ActionResult Index()
        {
            var model = new PageGridModel();
            return View(model);
        }
        public virtual JsonResult Telegram_Read([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));

            var query = Ioc.TelegramBiz.GetMessages();
            return Json(query.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}