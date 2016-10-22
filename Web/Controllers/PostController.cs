using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class PostController : BaseController
    {
        private readonly IPostBiz _postBiz;
        private readonly ICommentBiz _commentBiz;

        public PostController(IPostBiz postBiz, ICommentBiz commentBiz)
        {
            _postBiz = postBiz;
            _commentBiz = commentBiz;
        }

        // GET: Post
        public virtual ActionResult Index(string code)
        {
            if (string.IsNullOrEmpty(code))
                return RedirectToAction(MVC.Error.notfound());

            PostModel model;
            int id;
            if (int.TryParse(code, out id))
            {
                var dbpost = _postBiz.Get(id);
                dbpost.VisitCount = dbpost.VisitCount + 1;
                model = dbpost.ToViewModel<PostModel>();
                _postBiz.CreateEdit(dbpost);
            }
            else
            {
                var dbpost = _postBiz.Get(code);
                dbpost.VisitCount = dbpost.VisitCount + 1;
                model = dbpost.ToViewModel<PostModel>();
                _postBiz.CreateEdit(dbpost);
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
                return Json(_commentBiz.CreateEdit(comment).ToJOperationResult());
            }
            return Json(new JOperationResult { Status = false, Message = Common.Resources.General.IncorrectData });
        }
    }
}