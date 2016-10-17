using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Web.Models;
using Tazeyab.Web.WebLogic;

namespace Tazeyab.Web.Controllers
{
    public partial class PostController : BaseController
    {
        // GET: Post
        public virtual ActionResult Index(string code)
        {
            if (string.IsNullOrEmpty(code))
                return RedirectToAction(MVC.Error.notfound());

            PostModel model;
            int id;
            if (int.TryParse(code, out id))
            {
                var dbpost = Ioc.PostBiz.Get(id);
                dbpost.VisitCount = dbpost.VisitCount + 1;
                model = dbpost.ToViewModel<PostModel>();
                Ioc.PostBiz.CreateEdit(dbpost);
            }
            else
            {
                var dbpost = Ioc.PostBiz.Get(code);
                dbpost.VisitCount = dbpost.VisitCount + 1;
                model = dbpost.ToViewModel<PostModel>();
                Ioc.PostBiz.CreateEdit(dbpost);
            }

            if (model.PostType == PostType.Page)
                return View(MVC.Post.Views.Page, model);

            model.Content = model.Content.Replace("[[", "<").Replace("]]", ">");

            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        public virtual JsonResult SendComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                return Json(Ioc.CommentBiz.CreateEdit(comment).ToJOperationResult());
            }
            return Json(new JOperationResult { Status = false, Message = Common.Resource.General.IncorrectData });
        }
    }
}