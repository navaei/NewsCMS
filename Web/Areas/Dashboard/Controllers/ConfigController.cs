using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class ConfigController : BaseAdminController
    {
        private readonly IAppConfigBiz _appConfigBiz;

        public ConfigController(IAppConfigBiz appConfigBiz)
        {
            _appConfigBiz = appConfigBiz;
        }

        // GET: Dashboard/Setting
        public virtual ActionResult Index()
        {

            return View();
        }
        public virtual JsonResult Configs_Read([DataSourceRequest] DataSourceRequest request)
        {
            var query = _appConfigBiz.GetList();
            //var model = query.Select(c => new
            //{
            //    c.Id,
            //    c.Title,
            //    c.Value,
            //    c.Meaning,
            //});

            return Json(query.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult Manage(ProjectSetup model)
        {
            JOperationResult res = new JOperationResult() { Status = false };
            if (ModelState.IsValid)
            {
                res = _appConfigBiz.CreateEdit(model).ToJOperationResult();
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}