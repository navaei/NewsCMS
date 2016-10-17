using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;

namespace Tazeyab.Web.Controllers
{
    public partial class SearchController : BaseController
    {
        //
        // GET: /Search/

        public virtual ActionResult Index(string Content)
        {
            //#region ViewBag
            //ViewBag.Title = Content;
            //ViewBag.Content = Content;
            //ViewBag.SearchExpersion = Content;
            //ViewBag.PageHeader = "نتیجه جستجو " + Content;
            //#endregion
            return Redirect(string.Format(@"http://www.google.com/webhp?hl=fa#hl=fa&q=site:tazeyab.com+{0}", Content));
        }

    }
}
