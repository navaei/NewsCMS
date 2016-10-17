using Mn.Framework.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.Web.Models;
using Tazeyab.Web.WebLogic;

namespace Tazeyab.Web.Controllers
{
    public partial class PhotoController : BaseController
    {
        //
        // GET: /Photo/

        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = TazeyabConfig.Cache1Min)]
        public virtual ActionResult Today()
        {
            var model = new PhotoToday();
            model.Items = Ioc.ItemBiz.GetList().Where(i => i.HasPhoto).OrderByDescending(i => i.PubDate).Take(12).ToList();

            return View(model);
        }

        [OutputCache(Duration = TazeyabConfig.Cache1Min, VaryByParam = "offset")]
        public virtual JsonNetResult GetPhotos(int offset = 0)
        {
            for (int c = 2; c <= 10; c += 2)
            {
                var date = DateTime.Now.AddHours(-c);
                var items = Ioc.ItemBiz.GetList().Where(i => i.PubDate > date && i.HasPhoto).OrderByDescending(i => i.PubDate).Skip(offset).Take(12).ToList();
                if (items.Count() == 12)
                    return JsonNet(items.Select(p => new { p.Id, p.Title, p.PhotoUrl, p.SiteUrl, p.ItemId }));
            }

            return JsonNet(null);
        }

        [OutputCache(Duration = 300, VaryByParam = "cat")]
        public virtual ActionResult NewsPaper(string cat)
        {
            var CatCurrent = Ioc.CatBiz.Get("NewsPaper");

            #region ViewBag
            ViewBag.Title = CatCurrent.Title;
            ViewBag.ImageThumbnail = CatCurrent.ImageThumbnail;
            ViewBag.Content = CatCurrent.Code.Trim();
            ViewBag.PageHeader = "تازه ترین های " + CatCurrent.Title;
            ViewBag.KeyWords = !string.IsNullOrEmpty(CatCurrent.KeyWords) ? CatCurrent.KeyWords.Replace("-", ",") : "";
            ViewBag.Discription = CatCurrent.Description;
            #endregion

            var today = DateTime.Now;
            var res = Ioc.DataContext.PhotoItems.Where(x => x.CatId == CatCurrent.Id &&
                x.CreationDate.Value.Year == today.Year &&
                x.CreationDate.Value.Month == today.Month &&
                x.CreationDate.Value.Day == today.Day).ToList();
            if (!res.Any() && DateTime.Now.Hour > 6)
            {
                if (new CrawlerEngine.Updater.NewsPaperUpdater().StartNew("NewsPaper") > 2)
                    res = Ioc.DataContext.PhotoItems.Where(x => x.Id == CatCurrent.Id &&
                        x.CreationDate.Value.Year == today.Year &&
                        x.CreationDate.Value.Month == today.Month &&
                        x.CreationDate.Value.Day == today.Day).ToList();
                //new RobotClient().Execution(CommandList.UpdateNewsPaper);
            }
            else
            {
                res = res.Where(pi => (string.IsNullOrEmpty(cat) || (pi.Attribute as NewspaperItemAttribute).Cat == cat)).ToList();
            }

            return View(res);
        }

        public virtual FileContentResult CatPhoto(int Id)
        {
            // Skipping any validation etc -to read no-photo image [if data is not present] - for simplicity            
            byte[] ImagByte = Ioc.CatBiz.Get(Id).Icon;
            return new FileContentResult(ImagByte, "image/png");
        }

        public virtual FileContentResult GetPhoto(byte[] ImagByte)
        {
            return new FileContentResult(ImagByte, "image/png");

        }
    }
}
