using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.ExternalService;
using Mn.NewsCms.Web.WebLogic;
using Mn.NewsCms.Web.Models;




namespace Mn.NewsCms.Web.Controllers
{
    public partial class CatController : BaseController
    {
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IFeedItemBusiness _feedItemBusiness;
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly ITagBusiness _tagBusiness;
        private readonly IPostBiz _postBiz;
        private readonly ISiteBusiness _siteBusiness;
        private readonly IBlogService _blogService;
        private readonly IUnitOfWork _unitOfWork;

        public CatController(ICategoryBusiness categoryBusiness, IFeedItemBusiness feedItemBusiness,
            IAppConfigBiz appConfigBiz, ITagBusiness tagBusiness, IPostBiz postBiz, ISiteBusiness siteBusiness, IBlogService blogService, IUnitOfWork unitOfWork)
        {
            _categoryBusiness = categoryBusiness;
            _feedItemBusiness = feedItemBusiness;
            _appConfigBiz = appConfigBiz;
            _tagBusiness = tagBusiness;
            _postBiz = postBiz;
            _siteBusiness = siteBusiness;
            _blogService = blogService;
            _unitOfWork = unitOfWork;
        }

        [OutputCache(Duration = CmsConfig.Cache5Min, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult Index(string Content, int PageIndex)
        {
            var model = new CatItemsPageModel();
            var LastItemPubDate = DateTime.Now.AddMinutes(10);
            var catCurrent = _categoryBusiness.Get(Content);
            #region ViewBag
            ViewBag.EnTityRef = catCurrent.Id;
            ViewBag.Toggle = "1";
            ViewBag.SearchTextDir = "text-align:right;direction:rtl;display:none";
            ViewBag.Title = catCurrent.Title;
            ViewBag.ImageThumbnail = catCurrent.ImageThumbnail;
            ViewBag.Content = catCurrent.Code.Trim().ToLower();
            ViewBag.PageHeader = "اخبار " + catCurrent.Title;
            ViewBag.KeyWords = string.IsNullOrEmpty(catCurrent.KeyWords) ? "" : catCurrent.KeyWords.Replace("-", ",");
            ViewBag.Discription = catCurrent.Description;
            ViewBag.CatCurrent = catCurrent;
            ViewBag.PageIndex = PageIndex + 1;
            ViewBag.PageCount = 15;
            #endregion

            var allcats = _categoryBusiness.GetList(catCurrent.Id).ToList();
            allcats.Insert(0, catCurrent);
            var Id = catCurrent.Id;
            model.Items = _feedItemBusiness.FeedItemsByCat(Id, PageSize, PageIndex, false);
            model.VisualItems = _feedItemBusiness.FeedItemsByCat(Id, _appConfigBiz.GetVisualPostCount() + _appConfigBiz.GetVisualPostCount(), PageIndex, true);
            //-----------------Sub cat-------------------
            var SubCats = allcats.Where(x => x.ParentId == catCurrent.Id).ToList();
            SubCats.ForEach(x => x.ParentId = 0);
            if (SubCats.Count() > 0)
            {
                ViewBag.Categorys = SubCats;
                var items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "همه مطالب", Value = catCurrent.Code });
                foreach (var SubCat in SubCats)
                {
                    items.Add(new SelectListItem { Text = SubCat.Title, Value = SubCat.Code });
                }
                ViewBag.SubCats = items;
            }


            var allIds = allcats.Select(x => x.Id).ToList();
            ViewBag.RelatedTags = _tagBusiness.GetList()
                .Where(t => t.Categories.Any(tc => allIds.Contains(tc.Id))).ToList();
            //ViewBag.RelatedTags = context.Tags.Where(x => x.TagCategories.Where(c => c.Categorie_CatCurrent == ViewBag.CatCurrent.CatCurrent || c.Categorie_CatCurrent == ViewBag.CatCurrent.ParentId).Count() > 0).ToList();

            //----------------Top Site in this Cat-------                  
            ViewBag.TopSites =_unitOfWork.Database.SqlQueryCache_FirstParam<SiteOnlyTitle>(120, "Sites_Select_TopByCat {0},{1}", catCurrent.Id, 15).ToList();
            if (ViewBag.TopSites == null)
                ViewBag.TopSites = _siteBusiness.GetTopSites(18, 120);

            model.Posts = _postBiz.GetList().Where(p => !p.MetaData.IsDeleted && p.PublishDate < DateTime.Now && p.Categories.Any(pc => pc.Id == catCurrent.Id || pc.ParentId == catCurrent.Id))
                .Take(_appConfigBiz.GetVisualPostCount()).ToList();

            #region Tabs
            //ViewBag.RemoteWebParts = (from p in Ioc.DataContext.RemoteWebParts
            //                          where
            //                          p.Active == true &&
            //                          p.Categories.Any(x => x.Id == catCurrent.Id)
            //                          select p).ToList();

            //ViewBag.Pages = catCurrent.Posts.Where(p => p.PostType == PostType.Tab).ToList();

            #endregion

            return View("Index." + CmsConfig.ThemeName, model);
        }

        [AjaxOnly]
        [OutputCache(Duration = CmsConfig.Cache10Min, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult FeedItems(string Content, int PageIndex)
        {
            ViewBag.Content = Content;

            if (PageIndex > 5)
                PageIndex = 5;

            var res = _feedItemBusiness.FeedItemsByCat(ref Content, PageSize, PageIndex);
            #region viewBag
            ViewBag.PageIndex = PageIndex + 1;
            ViewBag.PageHeader = "تازه ترین های " + Content;
            #endregion

            return PartialView("_FeedItems.Tazeyab", res);
        }

        [OutputCache(Duration = CmsConfig.Cache5Min, VaryByParam = "Content")]
        public virtual ActionResult FeedItemsRemote(string Content, int PageSize)
        {
            PageSize = PageSize < 50 ? PageSize : 25;
            ViewBag.BaseAddress = "http://" + Resources.Core.SiteUrl + "/cat/" + Content;

            var res = _feedItemBusiness.FeedItemsByCat(ref Content, PageSize, 0);
            ViewBag.PageHeader = "تازه‌ترین‌های " + Content;
            var opr = _blogService.InsertRemoteRequestLog(this.Name, Content);
            ViewBag.Content = Content;
            return PartialView("_FeedItemsRemote", res);
        }

    }
}
