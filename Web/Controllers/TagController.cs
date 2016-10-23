using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.ExternalService;
using Mn.NewsCms.Web.WebLogic;
using Mn.NewsCms.Web.Models;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class TagController : BaseController
    {
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IFeedItemBusiness _feedItemBusiness;
        private readonly IFeedBusiness _feedBusiness;
        private readonly ITagBusiness _tagBusiness;
        private readonly IBlogService _blogService;
        private readonly IPostBiz _postBiz;
        private readonly IUnitOfWork _unitOfWork;

        public TagController(IAppConfigBiz appConfigBiz, ICategoryBusiness categoryBusiness, IFeedItemBusiness feedItemBusiness, IFeedBusiness feedBusiness,
            ITagBusiness tagBusiness, IBlogService blogService, IPostBiz postBiz, IUnitOfWork unitOfWork)
        {
            _appConfigBiz = appConfigBiz;
            _categoryBusiness = categoryBusiness;
            _feedItemBusiness = feedItemBusiness;
            _feedBusiness = feedBusiness;
            _tagBusiness = tagBusiness;
            _blogService = blogService;
            _postBiz = postBiz;
            _unitOfWork = unitOfWork;
        }

        [OutputCache(Duration = CmsConfig.Cache3Hour)]
        public virtual ActionResult All(int MaxCount = 200)
        {
            var res = new List<AllTagModel>();
            var cats = _categoryBusiness.GetList().Include(c => c.Tags).Where(c => c.Tags.Any());
            foreach (var cat in cats)
            {
                res.Add(new AllTagModel() { CatCode = cat.Code, CatTitle = cat.Title, Tags = cat.Tags.ToTagModel().ToList() });
            }

            return View("All." + CmsConfig.ThemeName, res);
        }

        [OutputCache(Duration = CmsConfig.Cache5Min, VaryByParam = "Content;PageIndex")]//, VaryByCustom = "IsMobile"
        public virtual ActionResult Index(string content, int PageIndex = 0)
        {
            var model = new TagItemsPageModel();
            content = content.Replace("_", " ");
            content = content.Replace("+", " ");
            var TagCurrent = new Tag();
            var LastItemPubDate = DateTime.Now.AddMinutes(15);
            //if (Content.EqualsX("All") || string.IsNullOrEmpty(Content))
            //    return RedirectToAction(MVC.Tag.Actions.ActionNames.All, MVC.Tag.Name);

            try
            {
                TagCurrent = _unitOfWork.Set<Tag>().SingleOrDefault(x => x.Title.Equals(content) || x.EnValue == content);
                //    if (TagCurrent.Value.Trim().Length != Content.Trim().Length && TagCurrent.EnValue.Trim().Length != Content.Trim().Length)
                //        return RedirectToAction(MVC.Key.ActionNames.Index, MVC.Key.Name, new { Content = Content, PageSize = PageSize, LastItemPubDate });
            }
            catch
            {
                var tagCurrents = _unitOfWork.Set<Tag>().Where(x => x.Value.StartsWith(content));
                if (tagCurrents.Any())
                    TagCurrent = tagCurrents.First();
                else
                    return RedirectToAction(MVC.Key.ActionNames.Index, MVC.Key.Name, new { Content = content, PageSize = PageSize, LastItemPubDate });
            }
            if (TagCurrent == null)
                return RedirectToAction(MVC.Key.ActionNames.Index, MVC.Key.Name, new { Content = content, PageSize = PageSize, LastItemPubDate });


            #region ViewBag
            //ViewBag.EntityCode = "Tag";
            ViewBag.EntityRef = TagCurrent.Id;
            //ViewBag.PageSize = PageSize;
            ViewBag.Toggle = "1";
            ViewBag.SearchTextDir = "text-align:right;direction:rtl";
            ViewBag.Title = "اخبار " + TagCurrent.Title;
            ViewBag.ImageThumbnail = TagCurrent.ImageThumbnail;
            ViewBag.HasBackgroundImage = TagCurrent.HasBackgroundImage;
            ViewBag.Content = string.IsNullOrEmpty(TagCurrent.EnValue) ? TagCurrent.Value.Trim() : TagCurrent.EnValue;
            ViewBag.SearchExpersion = "Tag:" + TagCurrent.Value;
            ViewBag.PageHeader = "تازه ترین های " + TagCurrent.Title;
            ViewBag.Discription = "تازه ترین های اخبار روز و مطالب " + TagCurrent.Title;
            ViewBag.KeyWords = "," + TagCurrent.EnValue + "," + TagCurrent.Value.Replace("|", ",") + ",اخبار روز,تازه ترین خبرها,خبرخوان";
            ViewBag.TagCurrent = TagCurrent;
            ViewBag.PageIndex = PageIndex + 1;
            #endregion
            if (string.IsNullOrEmpty(TagCurrent.Value) && string.IsNullOrEmpty(TagCurrent.EnValue))
            {
                return RedirectToAction(MVC.Tag.Name, MVC.Tag.ActionNames.All, null);
            }

            #region Body

            model.Items = _feedItemBusiness.FeedItemsByTag(TagCurrent, PageSize, PageIndex);
            model.VisualItems = _feedItemBusiness.FeedItemsByTag(TagCurrent, _appConfigBiz.GetVisualPostCount() + _appConfigBiz.GetVisualPostCount(), PageIndex, true);
            model.Posts = _postBiz.GetList().Where(p => !p.MetaData.IsDeleted && p.PublishDate < DateTime.Now && p.Tags.Any(tc => tc.Id == TagCurrent.Id || tc.ParentTagId == TagCurrent.Id))
                .Take(_appConfigBiz.GetVisualPostCount()).ToList();
            #endregion
            #region SitesTags
            //----------------Top Site in this Tag-------               
            var cats = TagCurrent.Categories.ToList();
            try
            {
                if (cats.Any())
                {
                    var catsid = cats.Select(x => x.Id).ToList();
                    var feeds = _feedBusiness.GetList().Where(f => f.Categories.Any(cf => catsid.Contains(cf.Id)));
                    var sites = feeds.Select(f => f.Site);
                    ViewBag.TopSites = sites.Distinct().Select(s => new SiteOnlyTitle() { SiteTitle = s.SiteTitle, SiteUrl = s.SiteUrl }).Take(20).ToList();
                }
                //ViewBag.TopSites = context.Database.SqlQuery<SiteOnlyTitle>("Sites_Select_TopByTag {0},{1}", TagCurrent.TagId, 15).ToList();
            }
            catch { }

            var relatedTags = cats.SelectMany(c => c.Tags.Select(t => t)).Distinct().Take(25).ToList();
            //relatedTags.Remove(TagCurrent.Id);
            ViewBag.RelatedTags = relatedTags;

            #endregion
            //B_feeditem.IncreaseVisitCount(res as IEnumerable<FeedItem>);
            model.Items = _feedItemBusiness.DescriptClear(model.Items, TagCurrent.Value).ToList();
            //_feedItemBusiness.Add_FeedItemtoCache(content, PageIndex, View(res), 60);

            #region Tabs
            //var webParts = Ioc.RemoteWpBiz.GetByTag(TagCurrent.Id).ToList();
            //if (webParts.Any())
            //    ViewBag.RemoteWebParts = webParts;
            //else
            //    if (TagCurrent.Categories.Any())
            //    ViewBag.RemoteWebParts = Ioc.RemoteWpBiz.GetByCats(TagCurrent.Categories.Select(c => c.Id).ToList()).ToList();

            //ViewBag.Pages = TagCurrent.Posts.Where(p => p.PostType == PostType.Tab).ToList();

            #endregion
            return View("Index." + CmsConfig.ThemeName, model);
        }

        [AjaxOnly]
        [OutputCache(Duration = CmsConfig.Cache30Min, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult FeedItems(string content, int PageIndex)
        {
            ViewBag.Content = content;
            var TagCurrent = _tagBusiness.Get(content);
            if (TagCurrent == null)
            {
                return RedirectToAction(MVC.Error.notfound());
            }

            #region viewBag
            ViewBag.PageIndex = PageIndex + 1;
            ViewBag.PageHeader = "تازه ترین های " + TagCurrent.Title;
            #endregion

            var res = _feedItemBusiness.FeedItemsByTag(TagCurrent, PageSize, PageIndex);
            return PartialView("_FeedItems.Tazeyab", res);
        }

        [OutputCache(Duration = CmsConfig.Cache10Min, VaryByParam = "Content")]
        public virtual ActionResult FeedItemsRemote(string content, int PageSize)
        {
            var currentTag = _tagBusiness.Get(content);
            if (currentTag == null)
            {
                return RedirectToAction(MVC.Error.notfound());
            }
            content = currentTag != null && string.IsNullOrEmpty(currentTag.EnValue) ? currentTag.Title : currentTag.EnValue;
            ViewBag.BaseAddress = @"http://" + Resources.Core.SiteUrl + "/tag/" + content;

            ViewBag.Content = content;

            var res = _feedItemBusiness.FeedItemsByTag(currentTag, PageSize, 0);
            _blogService.InsertRemoteRequestLog(this.Name, content);
            ViewBag.PageHeader = "تازه ترین های " + currentTag.Title;
            return PartialView("_FeedItemsRemote", res);
        }

        [OutputCache(Duration = CmsConfig.Cache1Hour, VaryByParam = "Content")]
        public virtual ActionResult RSS(string Content)
        {
            var LastItemPubDate = DateTime.Now.AddMinutes(10);
            var TagCurrent = _tagBusiness.Get(Content);
            if (TagCurrent == null)
            {
                return RedirectToAction(MVC.Error.notfound());
            }
            #region ViewBag
            ViewBag.PageSize = 10;
            ViewBag.Title = TagCurrent.Title;
            ViewBag.Content = string.IsNullOrEmpty(TagCurrent.EnValue) ? TagCurrent.Value.Trim() : TagCurrent.EnValue;
            ViewBag.SearchExpersion = "Tag:" + TagCurrent.Value;
            ViewBag.PageHeader = "تازه ترین مطالب مرتبط با " + TagCurrent.Value;
            #endregion
            var res = _feedItemBusiness.FeedItemsByTag(TagCurrent, PageSize, 0, 20);
            View(res);
            return View();
        }


    }
}
