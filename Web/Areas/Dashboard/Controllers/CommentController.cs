using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.Framework.Web.Model;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class CommentController : BaseAdminController
    {
        // GET: Dashboard/Comment
        public virtual ActionResult Index()
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Mn.NewsCms.Common.Resources.General.Edit, "/Dashboard/Comment/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Approve, "approveComment('#=Id#')"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Delete, "deleteGridRow('/Dashboard/Comment/Delete/#=Id#')"));
            return View(model);
        }
        public virtual JsonResult Comments_Read([DataSourceRequest] DataSourceRequest request, int? postId)
        {
            if (!request.Sorts.Any())
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));

            var query = Ioc.CommentBiz.GetList();
            if (postId.HasValue)
                query = query.Where(c => c.PostId == postId);

            return Json(query.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult Approve(int id)
        {
            var comment = Ioc.CommentBiz.Get(id);
            comment.Approve = true;
            var res = Ioc.CommentBiz.CreateEdit(comment);

            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult Delete(int id)
        {
            var res = Ioc.CommentBiz.Delete(id);
            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
    }
}