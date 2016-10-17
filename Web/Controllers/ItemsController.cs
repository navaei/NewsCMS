using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common.Models;
using Tazeyab.Common;
using Tazeyab.DomainClasses.UpdaterBusiness;
using Tazeyab.DomainClasses.ContentManagment;
using Tazeyab.Web.WebLogic;
using Kendo.Mvc.UI;
using Tazeyab.Web.Models;


namespace Tazeyab.Web.Controllers
{
    public partial class ItemsController : BaseController
    {
        public static List<FeedItem> VisitedItems = new List<FeedItem>();
        const int maxVisitedItems = 45;

        [OutputCache(Duration = 1200, VaryByParam = "EntityCode;EntityRef")]
        public virtual ActionResult MostVisitedItems(string EntityCode, int EntityRef, int PageSize)
        {
            PageSize = PageSize > 20 ? 20 : PageSize;
            var res = Ioc.ItemBiz.MostVisitedItems(EntityCode, EntityRef, PageSize, 30);
            ViewBag.PageHeader = "پر بیننده ترین های ";
            return PartialView("_MostVisitedItems", res);
        }

        public virtual ActionResult TodayMostVisitedItems()
        {
            var mosteItems = Ioc.ItemBiz.MostVisitedItems("today", 10, 15).ToList();
            var model = new HomeItemsPanelViewModel();
            model.Items = mosteItems.Select(i => new FeedItem()
            {
                Title = i.Title,
                Description = i.Description,
                Link = i.Link,
                Id = i.Id,
                ItemId = i.ItemId,
                SiteUrl = string.IsNullOrEmpty(i.SiteUrl) ? "v" : i.SiteUrl,
                SiteTitle = i.SiteTitle,
                PubDate = i.PubDate
            }).ToList();

            return View(MVC.Home.Views.IndexItem, new HomeItemsPanelViewModel()
            {
                Items = model.Items,
                Cat = new Category() { Code = "news", Title = "پر بازدیدترینها", Color = "transparent" },
                ShowMoreBtn = false
            });
        }

        [OutputCache(Duration = 600)]
        public virtual ActionResult TodayMostVisited()
        {
            var res = Ioc.ItemBiz.MostVisitedItems("today", 10, 30, 30);
            ViewBag.PageHeader = "پر بیننده ترین های امروز ";
            return PartialView(res);
        }

        [OutputCache(Duration = 60)]
        public virtual ActionResult Now()
        {
            ViewBag.PageHeader = "تازه ترین های وب ";
            ViewBag.Title = "همین حالا...";
            var membership = new TzMembership();
            var res = Ioc.ItemBiz.FeedItemsByTime(DateTime.Now.AddHours(DateTime.Now.NowHour()), 20, 0);
            if (res.Count() < 20)
            {
                var res2 = Ioc.ItemBiz.FeedItemsByTime(DateTime.Now.AddHours(DateTime.Now.NowHour() - 1), 20 - res.Count(), 0);
                res = res.Concat(res2).ToList();
            }
            return View("_FeedItems." + TazeyabConfig.ThemeName, res);
        }

        [OutputCache(Duration = 120)]
        public virtual ActionResult LastVisited()
        {
            if (VisitedItems.Count > maxVisitedItems)
            {
                lock (VisitedItems)
                {
                    VisitedItems.RemoveRange(0, maxVisitedItems / 2);
                }
            }
            return View(VisitedItems.Take(20).ToList());
        }
        [HttpPost]
        public void IncreaseVisitCount(string itemid)
        {
            Ioc.ItemBiz.IncreaseVisitCount(Guid.Parse(itemid));
        }

        [HttpGet]
        public virtual void AddTagToHistory(int TagId)
        {
            var tag = Ioc.TagBiz.Get(TagId);
            if (tag != null)
                Ioc.SearchHistoryBiz.Add(tag);
        }
    }
}
