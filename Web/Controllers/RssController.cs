using System.Linq;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Web.WebLogic;
using Mn.NewsCms.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class RssController : Controller
    {
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IFeedItemBusiness _feedItemBusiness;
        private readonly ITagBusiness _tagBusiness;

        public RssController(ICategoryBusiness categoryBusiness, IFeedItemBusiness feedItemBusiness, ITagBusiness tagBusiness)
        {
            _categoryBusiness = categoryBusiness;
            _feedItemBusiness = feedItemBusiness;
            _tagBusiness = tagBusiness;
        }

        //
        // GET: /Rss/
        TazehaContext context = new TazehaContext();
        [ManualActionCache(Duration = 600)]
        public virtual ActionResult Index(string Content, int PageSize)
        {
            var cat = _categoryBusiness.Get(Content);
            if (cat != null)
            {
                var allcats = _categoryBusiness.GetList(cat.Id).ToList();
                allcats.Insert(0, cat);
                var res = _feedItemBusiness.FeedItemsByCat(cat.Id, PageSize, 0);
                return new FeedResult("تازه ترین مطالب پیرامون  " + cat.Title, res.ToList());
            }
            var tag = _tagBusiness.Get(Content);
            if (tag != null)
            {
                var res = _feedItemBusiness.FeedItemsByTag(tag, PageSize, 0);
                return new FeedResult("تازه ترین مطالب پیرامون  " + tag.Title, res.ToList());
            }
            return View();
        }

    }
}
