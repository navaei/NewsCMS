using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common.Models;
using System.Data;
using Tazeyab.Common;
using Tazeyab.DomainClasses.ContentManagment;
using Tazeyab.WebLogic;
using Tazeyab.Web.WebLogic;
using Mn.Framework.Common;
using Tazeyab.Web.Models;




namespace Tazeyab.Web.Controllers
{
    public partial class CatController : BaseController
    {
        [OutputCache(Duration = TazeyabConfig.Cache5Min, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult Index(string Content, int PageIndex)
        {
            var model = new CatItemsPageModel();
            var LastItemPubDate = DateTime.Now.AddMinutes(10);
            var catCurrent = Ioc.CatBiz.Get(Content);
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

            var allcats = Ioc.CatBiz.GetList(catCurrent.Id).ToList();
            allcats.Insert(0, catCurrent);
            var Id = catCurrent.Id;
            model.Items = Ioc.ItemBiz.FeedItemsByCat(Id, PageSize, PageIndex, false);
            model.VisualItems = Ioc.ItemBiz.FeedItemsByCat(Id, Ioc.AppConfigBiz.GetVisualPostCount() + Ioc.AppConfigBiz.GetVisualPostCount(), PageIndex, true);
            //-----------------Sub cat-------------------
            var SubCats = allcats.Where(x => x.ParentId == catCurrent.Id).ToList();
            SubCats.ForEach(x => x.ParentId = 0);
            if (SubCats.Count() > 0)
            {
                ViewBag.Categorys = SubCats;
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "همه مطالب", Value = catCurrent.Code });
                foreach (var SubCat in SubCats)
                {
                    items.Add(new SelectListItem { Text = SubCat.Title, Value = SubCat.Code });
                }
                ViewBag.SubCats = items;
            }


            var allIds = allcats.Select(x => x.Id).ToList();
            ViewBag.RelatedTags = Ioc.TagBiz.GetList()
                .Where(t => t.Categories.Any(tc => allIds.Contains(tc.Id))).ToList();
            //ViewBag.RelatedTags = context.Tags.Where(x => x.TagCategories.Where(c => c.Categorie_CatCurrent == ViewBag.CatCurrent.CatCurrent || c.Categorie_CatCurrent == ViewBag.CatCurrent.ParentId).Count() > 0).ToList();

            //----------------Top Site in this Cat-------                  
            ViewBag.TopSites = Ioc.DataContext.Database.SqlQueryCache_FirstParam<SiteOnlyTitle>(120, "Sites_Select_TopByCat {0},{1}", catCurrent.Id, 15).ToList();
            if (ViewBag.TopSites == null)
                ViewBag.TopSites = Ioc.SiteBiz.GetTopSites(18, 120);

            model.Posts = Ioc.PostBiz.GetList().Where(p => !p.MetaData.IsDeleted && p.PublishDate < DateTime.Now && p.Categories.Any(pc => pc.Id == catCurrent.Id || pc.ParentId == catCurrent.Id))
                .Take(Ioc.AppConfigBiz.GetVisualPostCount()).ToList();

            #region Tabs
            //ViewBag.RemoteWebParts = (from p in Ioc.DataContext.RemoteWebParts
            //                          where
            //                          p.Active == true &&
            //                          p.Categories.Any(x => x.Id == catCurrent.Id)
            //                          select p).ToList();

            //ViewBag.Pages = catCurrent.Posts.Where(p => p.PostType == PostType.Tab).ToList();

            #endregion

            return View("Index." + TazeyabConfig.ThemeName, model);
        }

        [AjaxOnly]
        [OutputCache(Duration = TazeyabConfig.Cache10Min, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult FeedItems(string Content, int PageIndex)
        {
            ViewBag.Content = Content;

            if (PageIndex > 5)
                PageIndex = 5;

            var res = Ioc.ItemBiz.FeedItemsByCat(ref Content, PageSize, PageIndex);
            #region viewBag
            ViewBag.PageIndex = PageIndex + 1;
            ViewBag.PageHeader = "تازه ترین های " + Content;
            #endregion

            return PartialView("_FeedItems.Tazeyab", res);
        }

        [OutputCache(Duration = TazeyabConfig.Cache5Min, VaryByParam = "Content")]
        public virtual ActionResult FeedItemsRemote(string Content, int PageSize)
        {
            PageSize = PageSize < 50 ? PageSize : 25;
            ViewBag.BaseAddress = "http://" + Resources.Core.SiteUrl + "/cat/" + Content;

            var res = Ioc.ItemBiz.FeedItemsByCat(ref Content, PageSize, 0);
            ViewBag.PageHeader = "تازه‌ترین‌های " + Content;
            var opr = Ioc.BlogService.InsertRemoteRequestLog(this.Name, Content);
            ViewBag.Content = Content;
            return PartialView("_FeedItemsRemote", res);
        }


    }
}
