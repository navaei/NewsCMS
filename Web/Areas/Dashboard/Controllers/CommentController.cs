using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class CommentController : BaseAdminController
    {
        private readonly ICommentBiz _commentBiz;

        public CommentController(ICommentBiz commentBiz)
        {
            _commentBiz = commentBiz;
        }

        public virtual ActionResult Index()
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Common.Resources.General.Edit, "/Dashboard/Comment/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Common.Resources.General.Approve, "approveComment('#=Id#')"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Common.Resources.General.Delete, "deleteGridRow('/Dashboard/Comment/Delete/#=Id#')"));
            return View(model);
        }
        public virtual JsonResult Comments_Read([DataSourceRequest] DataSourceRequest request, int? postId)
        {
            if (!request.Sorts.Any())
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));

            var query = _commentBiz.GetList();
            if (postId.HasValue)
                query = query.Where(c => c.PostId == postId);

            return Json(query.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult Approve(int id)
        {
            var comment = _commentBiz.Get(id);
            comment.Approve = true;
            var res = _commentBiz.CreateEdit(comment);

            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult Delete(int id)
        {
            var res = _commentBiz.Delete(id);
            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
    }
}