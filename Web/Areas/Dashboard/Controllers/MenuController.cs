using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common.Navigation;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class MenuController : BaseAdminController
    {
        private readonly IMenuBiz _menuBiz;

        public MenuController(IMenuBiz menuBiz)
        {
            _menuBiz = menuBiz;
        }

        // GET: Dashboard/Menu
        public virtual ActionResult Index(int? menuId)
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Mn.NewsCms.Common.Resources.General.Edit, "/Dashboard/Ads/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Delete, "deleteGridRow('/Dashboard/Ads/Delete/#=Id#')"));

            var menus = _menuBiz.GetList().ToList();
            ViewBag.SelectedMenu = menuId.HasValue ? menuId.Value : (int)menus.First().Id;
            ViewBag.Menus = new SelectList(menus.Select(m => new { Value = m.Id.ToString(), Text = m.Title }).ToList(), "Value", "Text", (int)ViewBag.SelectedMenu);
            ViewBag.Items = menuId.HasValue ? _menuBiz.GetItems(menuId.Value).ToList() : _menuBiz.Get(MenuLocation.Top).MenuItems.ToList();
            return View(model);
        }
        public virtual JsonResult MenuItem_Read([DataSourceRequest] DataSourceRequest request, int? menuId)
        {
            if (request.Sorts != null && !request.Sorts.Any())
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));

            var query = menuId.HasValue && menuId.Value > 0 ? _menuBiz.GetList().SingleOrDefault(m => m.Id == menuId.Value).MenuItems : _menuBiz.Get(MenuLocation.Top).MenuItems;
            if (query == null)
                query = new List<Mn.NewsCms.Common.Navigation.MenuItem>();
            return Json(query.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult ManageItem(int id)
        {
            var item = _menuBiz.GetItems().SingleOrDefault(i => i.Id == id);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult ManageItem(Mn.NewsCms.Common.Navigation.MenuItem item)
        {
            var res = _menuBiz.CreateEdit(item);

            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult DeleteItem(int id)
        {
            var res = _menuBiz.DeleteItem(id);
            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
    }
}