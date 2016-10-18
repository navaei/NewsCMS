using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Mn.Framework.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common.Navigation;

namespace Tazeyab.Web.Areas.Dashboard.Controllers
{
    public partial class MenuController : BaseAdminController
    {
        // GET: Dashboard/Menu
        public virtual ActionResult Index(int? menuId)
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Mn.NewsCms.Common.Resource.General.Edit, "/Dashboard/Ads/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resource.General.Delete, "deleteGridRow('/Dashboard/Ads/Delete/#=Id#')"));

            var menus = Ioc.MenuBiz.GetList().ToList();
            ViewBag.SelectedMenu = menuId.HasValue ? menuId.Value : (int)menus.First().Id;
            ViewBag.Menus = new SelectList(menus.Select(m => new { Value = m.Id.ToString(), Text = m.Title }).ToList(), "Value", "Text", (int)ViewBag.SelectedMenu);
            ViewBag.Items = menuId.HasValue ? Ioc.MenuBiz.GetItems(menuId.Value).ToList() : Ioc.MenuBiz.Get(MenuLocation.Top).MenuItems.ToList();
            return View(model);
        }
        public virtual JsonResult MenuItem_Read([DataSourceRequest] DataSourceRequest request, int? menuId)
        {
            if (request.Sorts != null && !request.Sorts.Any())
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));

            var query = menuId.HasValue && menuId.Value > 0 ? Ioc.MenuBiz.GetList().SingleOrDefault(m => m.Id == menuId.Value).MenuItems : Ioc.MenuBiz.Get(MenuLocation.Top).MenuItems;
            if (query == null)
                query = new List<Tazeyab.Common.Navigation.MenuItem>();
            return Json(query.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult ManageItem(int id)
        {
            var item = Ioc.MenuBiz.GetItems().SingleOrDefault(i => i.Id == id);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult ManageItem(Tazeyab.Common.Navigation.MenuItem item)
        {
            var res = Ioc.MenuBiz.CreateEdit(item);

            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult DeleteItem(int id)
        {
            var res = Ioc.MenuBiz.DeleteItem(id);
            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
    }
}