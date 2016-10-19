using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.DomainClasses.ContentManagment;
using Mn.NewsCms.Web.WebLogic;
using Mn.NewsCms.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class RssController : Controller
    {
        //
        // GET: /Rss/
        TazehaContext context = new TazehaContext();
        [ManualActionCache(Duration = 600)]
        public virtual ActionResult Index(string Content, int PageSize)
        {
            var cat = Ioc.CatBiz.Get(Content);
            if (cat != null)
            {
                var allcats = Ioc.CatBiz.GetList(cat.Id).ToList();
                allcats.Insert(0, cat);
                var res = Ioc.ItemBiz.FeedItemsByCat(cat.Id, PageSize, 0);
                return new FeedResult("تازه ترین مطالب پیرامون  " + cat.Title, res.ToList());
            }
            var tag = Ioc.TagBiz.Get(Content);
            if (tag != null)
            {
                var res = Ioc.ItemBiz.FeedItemsByTag(tag, PageSize, 0);
                return new FeedResult("تازه ترین مطالب پیرامون  " + tag.Title, res.ToList());
            }
            return View();
        }

    }
}
