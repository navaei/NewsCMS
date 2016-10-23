using System;
using System.Linq;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.WebLogic;
using Mn.NewsCms.Web.WebLogic.BaseController;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class PhotoController : BaseController
    {
        private readonly IFeedItemBusiness _itemBiz;
        private readonly ICategoryBusiness _categoryBusiness;

        public PhotoController(IFeedItemBusiness itemBiz, ICategoryBusiness categoryBusiness)
        {
            _itemBiz = itemBiz;
            _categoryBusiness = categoryBusiness;
        }

        //
        // GET: /Photo/

        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = CmsConfig.Cache1Min)]
        public virtual ActionResult Today()
        {
            var model = new PhotoToday();
            model.Items = _itemBiz.GetList().Where(i => i.HasPhoto).OrderByDescending(i => i.PubDate).Take(12).ToList();

            return View(model);
        }

        [OutputCache(Duration = CmsConfig.Cache1Min, VaryByParam = "offset")]
        public virtual WebLogic.JsonNetResult GetPhotos(int offset = 0)
        {
            for (int c = 2; c <= 10; c += 2)
            {
                var date = DateTime.Now.AddHours(-c);
                var items = _itemBiz.GetList().Where(i => i.PubDate > date && i.HasPhoto).OrderByDescending(i => i.PubDate).Skip(offset).Take(12).ToList();
                if (items.Count() == 12)
                    return JsonNet(items.Select(p => new { p.Id, p.Title, p.PhotoUrl, p.SiteUrl, p.ItemId }));
            }

            return JsonNet(null);
        }

        public virtual FileContentResult CatPhoto(int Id)
        {
            // Skipping any validation etc -to read no-photo image [if data is not present] - for simplicity            
            byte[] ImagByte = _categoryBusiness.Get(Id).Icon;
            return new FileContentResult(ImagByte, "image/png");
        }

        public virtual FileContentResult GetPhoto(byte[] ImagByte)
        {
            return new FileContentResult(ImagByte, "image/png");

        }
    }
}
