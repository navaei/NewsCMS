using Mn.Framework.Common;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Mn.NewsCms.Common.Config;

namespace Mn.NewsCms.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {


            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            routes.MapRoute("sitemap.xml", "sitemap.xml", new { controller = "sitemap", action = "sitemap" });
            routes.MapRoute("sitemap", "sitemap", new { controller = "sitemap", action = "sitemap" });

            routes.MapRoute(
            "Items", // Route name
            "{controller}/Items/{Content}/{PageIndex}", // URL with parameters
            new { action = "FeedItems", PageSize = 25, PageIndex = 0 },
            new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
           "RemoteFeedItems", // Route name
           "{controller}/itemsremote/{Content}", // URL with parameters
           new { action = "FeedItemsRemote", PageSize = 20 });
            routes.MapRoute(
            "Rss", // Route name
            "rss/{Content}", // URL with parameters
            new { controller = "Rss", action = "index", PageSize = 30 });

            #if DEBUG
            
            #else
                    if (!ServiceFactory.Get<IAppConfigBiz>().ChkAppPrvcy()) return;
            #endif

            routes.MapRoute(
              "Review",
              "review",
              new { controller = "Home", action = "Review" },
              new[] { "Mn.NewsCms.Web.Controllers" });


            //---------Default tag cat site---------
            routes.MapRoute(
           "CatDefault",
           "cat",
           new { controller = "tag", action = "all", MaxCount = 500 });

            routes.MapRoute(
            "TagAll",
            "tag/all",
            new { controller = "tag", action = "all", MaxCount = 500 });

            routes.MapRoute(
           "TagDefault",
           "tag",
           new { controller = "tag", action = "all", MaxCount = 500 });

            routes.MapRoute(
           "SiteAll",
           "Site/All",
           new { controller = "site", action = "all", LastSiteId = UrlParameter.Optional }, // Parameter defaults  
           new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
          "SiteDefault",
          "Site",
          new { controller = "site", action = "all", LastSiteId = UrlParameter.Optional },// Parameter defaults   
          new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
          "Cat",
          "cat/{Content}/{PageIndex}",
          new { controller = "cat", action = "index", PageIndex = 0 },
          new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
           "Tag",
           "tag/{Content}/{PageIndex}",
           new { controller = "tag", action = "index", Content = string.Empty, PageIndex = 0 },
           new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
            "Key",
            "key/{content}/{PageIndex}",
            new { controller = "key", action = "index", PageIndex = 0 },
            new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
              "Site",
              "site/{Content}",
              new { controller = "site", action = "index", PageIndex = 0 },
              new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
            "SitePage", // Route name
            "site/{SiteLink}/{FeedItemId}/{*ItemTitle*}", // URL with parameters
            new { controller = "site", action = "page", ItemTitle = UrlParameter.Optional },
            new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
            "Login",
            "login/",
            new { controller = "account", action = "login" },
            new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
            "Register",
            "register/",
            new { controller = "account", action = "register" },
            new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
              "sendComment",
              "post/sendcomment",
              new { controller = "post", action = "sendcomment" },
              new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
               "post",
               "post/{code}/{title}",
               new { controller = "post", action = "index", title = UrlParameter.Optional },
               new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
             "Paging",
             "q/{controller}/{Content}/{PageIndex}",
             new { action = "index", PageIndex = 0 });

            routes.MapRoute(
             "Page",
             "page/{pageName}/{tab}",
             new { controller = "page", action = "index", tab = UrlParameter.Optional },
             new[] { "Mn.NewsCms.Web.Controllers" });

            routes.MapRoute(
               "Default", // Route name
               "{controller}/{action}/{id}", // URL with parameters
               new { controller = "home", action = "index", id = UrlParameter.Optional },
                new[] { "Mn.NewsCms.Web.Controllers" });
        }

    }
}