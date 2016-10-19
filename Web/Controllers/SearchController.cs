using System.Web.Mvc;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class SearchController : BaseController
    {
        //
        // GET: /Search/
        public virtual ActionResult Index(string Content)
        {           
            return Redirect($@"http://www.google.com/webhp?hl=fa#hl=fa&q=site:tazeyab.com+{Content}");
        }

    }
}
