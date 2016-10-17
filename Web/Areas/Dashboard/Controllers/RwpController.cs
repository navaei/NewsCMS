using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Web.Areas.Dashboard.Models;
using Tazeyab.Web.Models;
using Mn.Framework.Common.Model;
using Mn.Framework.Common;
using Mn.Framework.Web.Model;
using Kendo.Mvc;

namespace Tazeyab.Web.Areas.Dashboard.Controllers
{
    public partial class RwpController : BaseAdminController
    {
        public virtual ActionResult Index()
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Common.Resource.General.Edit, "/Dashboard/Rwp/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Common.Resource.General.Delete, "deleteGridRow('/Dashboard/Rwp/Delete/#=Id#')"));
            return View(model);
        }
        public virtual JsonResult Rwp_Read([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
                request.Sorts.Add(new SortDescriptor() { Member = "Id", SortDirection = System.ComponentModel.ListSortDirection.Descending });

            var query = Ioc.RemoteWpBiz.GetList().Select(wp => new { wp.Active, wp.CacheTime, wp.Id, wp.Url, wp.WebPartCode, wp.WebPartDesc, wp.WebPartTitle });
            //var cats = query.Select(c => new CategoryViewModel() { Id = c.Id, Code = c.Code, Title = c.Title, Color = c.Color });
            return Json(query.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult Manage(int? Id)
        {
            RwpModel model;
            if (Id.HasValue)
            {
                var dbRwp = Ioc.RemoteWpBiz.GetList().SingleOrDefault(a => a.Id == Id.Value);
                model = dbRwp.ToViewModel<RwpModel>();
                model.SelectedCategories = dbRwp.Categories.Select(c => c.Id).ToList();
                model.SelectedTags = dbRwp.Tags.Select(c => c.Id).ToList();
                ViewBag.Tags = dbRwp.Tags.Select(c => new TagViewModel { Id = c.Id, Title = c.Title }).ToList();
            }
            else
                model = new RwpModel();

            var cats = Ioc.CatBiz.GetList().Where(c => c.Active).Select(c => new MnTitleValue() { Title = c.Title, Value = c.Id }).ToList();
            ViewBag.Categories = cats;

            return View(model);
        }

        [HttpPost]
        public virtual JsonResult Manage(RwpModel model)
        {
            var res = new OperationStatus();
            RemoteWebPart rwp = new RemoteWebPart();
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    var dbRwp = Ioc.RemoteWpBiz.GetList().SingleOrDefault(a => a.Id == model.Id);
                    rwp = model.ToViewModel<RemoteWebPart>(dbRwp);
                }
                else
                    rwp = model.ToViewModel<RemoteWebPart>();

                rwp.WebPartCode = rwp.WebPartCode.ToLower();

                if (model.SelectedCategories != null)
                    rwp.Categories.AddEntities(Ioc.CatBiz.GetList(model.SelectedCategories.ToList()).ToList());
                if (model.SelectedTags != null)
                    rwp.Tags.AddEntities(Ioc.TagBiz.GetList().Where(t => model.SelectedTags.Contains(t.Id)).ToList());

                res = Ioc.RemoteWpBiz.CreateEdit(rwp);
            }

            var jres = res.ToJOperationResult();
            if (res.Status)
                jres.Data = rwp.Id.ToString();

            return Json(jres, JsonRequestBehavior.AllowGet);
        }
    }
}