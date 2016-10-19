using System.Web.Mvc;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class AdsController : BaseController
    {
        //
        // GET: /Ads/
        [OutputCache(Duration = 1000, VaryByParam = "catId,tagId")]
        public virtual ActionResult VerticalMenu(int? catId, int? tagId, int width = 0)
        {
            string res = string.Format("<div {0} >", width > 0 ? "style=Width:'" + width + 10 + "'px" : string.Empty);
            res += Ioc.AdsBiz.GetAds(width, tagId, catId);
            res += "</div>";
            ViewBag.AdsContent = res;
            return View();
        }

        [OutputCache(Duration = 1200)]
        public virtual ActionResult VerticalMenuBottom()
        {
            return View();
        }      

    }
}
