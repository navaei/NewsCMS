using System.Web.Mvc;
using Mn.NewsCms.DomainClasses.WebPartBusiness;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class WebPartController : BaseController
    {
        //
        // GET: /Ads/
        public virtual ActionResult VerticalMenu(string Content, int Width = 190)
        {
            string res = "<Div style='Width:" + Width + "px' >";
            WebPartManager ads = new WebPartManager();
            res += ads.getWebParts(Content, Width);
            res += "</Div>";
            ViewBag.webPartContent = res;
            return View();
        }

    }
}
