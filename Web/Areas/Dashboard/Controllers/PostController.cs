using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Web.Models;
using Mn.Framework.Web.Model;
using System.Text.RegularExpressions;
using Mn.NewsCms.Common.Helper;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class PostController : BaseAdminController
    {
        // GET: Dashboard/Post
        public virtual ActionResult Index(PostType type = PostType.News)
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Mn.NewsCms.Common.Resources.General.Edit, "/Dashboard/Post/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Delete, "deleteGridRow('/Dashboard/Post/Delete/#=Id#')"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, "تغییر متن", "openModal('/Dashboard/Post/UpdateContent/#=Id#')"));
            return View(model);
        }
        public virtual JsonResult Posts_Read([DataSourceRequest] DataSourceRequest request, int? catId)
        {
            var query = Ioc.PostBiz.GetList();
            if (catId.HasValue)
                query = query.Where(p => p.Categories.Any(c => c.Id == catId));
            var orders = query.Select(p => new
            {
                Id = p.Id,
                EnableComment = p.EnableComment,
                PublishDate = p.PublishDate,
                Categories = p.Categories.Select(c => new MnTitleValue() { Title = c.Title, Value = c.Id }).ToList(),
                TTags = p.Tags.Select(c => new MnTitleValue() { Title = c.Title, Value = c.Id }).ToList(),
                Title = p.Title,
                UserId = p.UserId
            });
            return Json(orders.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual ActionResult Manage(int? Id, string name)
        {
            PostModel model;
            if (Id.HasValue || !string.IsNullOrEmpty(name))
            {
                Post dbPost;
                if (Id.HasValue)
                    dbPost = Ioc.PostBiz.Get(Id.Value);
                else
                    dbPost = Ioc.PostBiz.Get(name);

                model = dbPost.ToViewModel<PostModel>();
                model.SelectedCategories = dbPost.Categories.Select(c => c.Id).ToList();
                model.SelectedTags = dbPost.Tags.Select(c => c.Id).ToList();
                model.PostImage = string.IsNullOrEmpty(dbPost.PostImage) ? "" : string.Format("<img src='{0}' />", dbPost.PostImage);
                ViewBag.Tags = dbPost.Tags.Select(c => new TagViewModel { Id = c.Id, Title = c.Title }).ToList();
            }
            else
                model = new PostModel() { UserId = UserId };

            var cats = Ioc.CatBiz.GetList().Where(c => c.Active).Select(c => new MnTitleValue() { Title = c.Title, Value = c.Id }).ToList();
            ViewBag.Categories = cats;
            //if (!model.SelectedCategories.Any())
            //    model.SelectedCategories.Add(cats.First().Value);

            return View(model);
        }
        public virtual ActionResult Delete(int? Id)
        {
            return View();
        }

        [HttpPost]
        public virtual JsonResult Manage(PostModel model)
        {
            OperationStatus res = new OperationStatus();
            if (ModelState.IsValid)
            {
                Post post;
                if (model.Id > 0)
                {
                    var dbPost = Ioc.PostBiz.Get(model.Id);
                    var oldContent = dbPost.Content + "";
                    post = model.ToModel<Post>(dbPost);
                }
                else
                    post = model.ToModel<Post>();

                if (model.SelectedCategories.Any())
                    post.Categories.AddEntities(Ioc.CatBiz.GetList(model.SelectedCategories.ToList()).ToList());
                if (model.SelectedTags.Any())
                    post.Tags.AddEntities(Ioc.TagBiz.GetList().Where(t => model.SelectedTags.Contains(t.Id)).ToList());

                post.Content = HttpUtility.HtmlDecode(post.Content);// post.PostType == PostType.Tab ? oldContent : HttpUtility.HtmlDecode(post.Content);

                if (!string.IsNullOrEmpty(model.PostImage) && HttpUtility.HtmlDecode(model.PostImage).ToLower().Contains("src"))
                {
                    post.PostImage = Regex.Match(HttpUtility.HtmlDecode(model.PostImage), "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value.ToLower();
                }
                else
                    post.PostImage = string.Empty;

                res = Ioc.PostBiz.CreateEdit(post);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult UpdateContent(int id)
        {
            var dbPost = Ioc.PostBiz.Get(id);
            return View(dbPost.ToViewModel<PostModel>());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual JsonResult UpdateContent(int id, string content)
        {
            var dbPost = Ioc.PostBiz.Get(id);
            dbPost.Content = HttpUtility.HtmlDecode(content);
            var res = Ioc.PostBiz.CreateEdit(dbPost);
            return Json(res.ToJOperationResult());
        }
    }
}