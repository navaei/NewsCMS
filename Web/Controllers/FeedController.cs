using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common.Models;
using System.Net;
using System.IO;
using Tazeyab.Common;
using System.Data.Entity.Infrastructure;
using Mn.Framework.Common;
using Tazeyab.Web.WebLogic;
using Tazeyab.DomainClasses;

namespace Tazeyab.Web.Controllers
{
    public partial class FeedController : BaseController
    {
        // string Tag, string SiteURL, string Cat
        [OutputCache(Duration = 1200, VaryByParam = "q;LastItemPubDate")]
        public virtual ActionResult FeedItems(decimal? FeedId, string q, string Content, Nullable<DateTime> LastItemPubDate, int PageSize = 15)
        {
            LastItemPubDate = LastItemPubDate.HasValue ? LastItemPubDate : DateTime.Now.AddMinutes(15);
            ViewBag.Content = Content;
            ViewBag.q = q = q.Trim();
            ViewBag.PageSize = PageSize;
            ViewBag.Toggle = "0";

            //context = new TazehaContext();
            ViewBag.SearchTextDir = "text-align:right;direction:rtl";
            char[] spliter = { ':' };
            string[] arr = q.Split(spliter);
            string Command = string.Empty;
            string Parameter = string.Empty;

            if (string.IsNullOrEmpty(q) && string.IsNullOrEmpty(Content))
            {
                return RedirectToAction("Index", "Home", null);
            }

            if (arr.Length == 1)
            {
                if (string.IsNullOrEmpty(Content))
                {
                    Command = "KEY";
                    Parameter = arr[0];
                }
                else
                {
                    Command = Content;
                    Parameter = q;
                }
            }
            if (arr.Length == 2)
            {
                Command = arr[0];
                Parameter = arr[1];
            }
            IEnumerable<FeedItem> s;
            switch (Command.ToUpper())
            {
                case "FEEDID":
                    s = Ioc.DataContext.Database.SqlQuery<FeedItem>("FeedItems_SELECT {0}", Parameter);
                    View(s);
                    break;
                case "KEY":
                    return RedirectToAction(MVC.Key.ActionNames.Index, MVC.Key.Name, new { Content = Parameter, PageSize = PageSize, LastItemPubDate = LastItemPubDate });
                    s = Ioc.DataContext.Database.SqlQuery<FeedItem>("FeedItems_SELECT_ByKeys {0},{1},{2},{3}", Parameter, PageSize, LastItemPubDate, User.Identity.IsAuthenticated && User != null ? WebSecurity.GetUser(User.Identity.Name).ProviderUserKey : null, LastItemPubDate).ToList();
                    ViewBag.SearchExpersion = Parameter;
                    ViewBag.PageHeader = "نتایج جستجو مربوط به عبارت:  " + Parameter;
                    View(s);
                    //-----------------------search histroy----------------
                    //context.SearchHistories.Add(new SearchHistory { SearchKey = Parameter, UserId = User.Identity.IsAuthenticated ? Guid.Parse(WebSecurity.GetUser(User.Identity.Name).ProviderUserKey.ToString()) : Guid.Empty });
                    break;
                case "TAG":
                    return RedirectToAction(MVC.Tag.ActionNames.Index, MVC.Tag.Name, new { Content = Parameter, PageSize = PageSize, LastItemPubDate = LastItemPubDate });
                    s = Ioc.DataContext.Database.SqlQuery<FeedItem>("FeedItems_SELECT_ByKeys {0},{1},{2},{3}", Parameter, PageSize, LastItemPubDate, User.Identity.IsAuthenticated && User != null ? WebSecurity.GetUser(User.Identity.Name).ProviderUserKey : null).ToList();
                    ViewBag.SearchExpersion = Parameter;
                    ViewBag.PageHeader = "نتایج جستجو مربوط به عبارت:  " + Parameter;
                    View(s);
                    //----------------set Textbox direction------
                    ViewBag.SearchTextDir = "text-align:right;direction:rtl";
                    //----------------Top Site in this Cat-------
                    ViewBag.TopSites = context.Database.SqlQuery<Site>("Sites_Select_TopByTag {0},{1}", Parameter, 15).ToList();
                    //---------------Related Tags-----------------
                    ViewBag.RelatedTags = context.Database.SqlQuery<Tag>("Tags_SELECT_Related {0},{1}", Parameter, 30).ToList();
                    //-----------------------search histroy----------------
                    //int TagId = context.Tags.SingleOrDefault(x => x.Value.Equals(Parameter)).TagId;
                    //context.SearchHistories.Add(new SearchHistory { TagId = TagId, UserId = User.Identity.IsAuthenticated ? Guid.Parse(WebSecurity.GetUser(User.Identity.Name).ProviderUserKey.ToString()) : Guid.Empty });
                    break;

                case "SITEURL":
                    return RedirectToAction(MVC.Site.ActionNames.Index, MVC.Site.Name, new { Content = Parameter, PageSize = PageSize, LastItemPubDate = LastItemPubDate });
                    var siteCurrent = Ioc.DataContext.Sites.Where(x => x.SiteUrl.StartsWith(Parameter)).First();
                    s = Ioc.DataContext.Database.SqlQuery<FeedItem>("FeedItems_SELECT_BySiteID {0},{1},{2}", siteCurrent.Id, PageSize, LastItemPubDate).ToList();
                    //var itemcount = context.Database.ExecuteSqlCommand("FeedItems_SELECT_BySiteID_COUNT @SiteID",new sql siteCurrent.Id);
                    ViewBag.PageHeader = "تازه ترین مطالب سایت:" + Parameter + "   " + "امتیاز در گوگل: " + siteCurrent.PageRank;
                    ViewBag.PageHeader += " تعداد مطالب موجود " + context.FeedItems.Count(x => x.Feed.Id == siteCurrent.Id).ToString();
                    View(s);
                    //----------------set Textbox direction------
                    ViewBag.SearchTextDir = "text-align:left;direction:ltr";
                    break;

                case "CAT":
                    return RedirectToAction(MVC.Cat.ActionNames.Index, MVC.Cat.Name, new { Content = Parameter, PageSize = PageSize, LastItemPubDate = LastItemPubDate });
                    break;
            }
            View(Ioc.ItemBiz.DescriptClear(((System.Web.Mvc.ViewResultBase)(View())).Model as List<FeedItem>, Parameter));
            if (Command.ToUpper() == "KEY")
                ViewBag.SearchExpersion = Parameter;
            else if (Command.ToUpper() == "TAG")
                ViewBag.SearchExpersion = Command + ":" + Parameter;
            else if (Command.ToUpper() == "CAT")
            {
                ViewBag.SearchExpersion = "CAT:" + Parameter;
                //q="دسته :"+Parameter;
            }
            else
                ViewBag.SearchExpersion = Command + ":" + Parameter; //string.Empty;
            //else
            //    ViewBag.SearchExpersion = Command + ":" + Parameter;

            //--------menu------------
            ViewBag.TopSites = ViewBag.TopSites == null ? Ioc.SiteBiz.GetTopSites(20, 120) : ViewBag.TopSites;
            ViewBag.Categorys = ViewBag.Categorys == null ? Ioc.CatBiz.AllCats_Cache() : ViewBag.Categorys;
            return View();
        }
        public virtual JsonResult UpdateForm(string sortBy)
        {
            string result = "Your result here";
            return Json(result);
        }
        public virtual ViewResult Autocomplete()
        {
            return View();
        }

    }
}