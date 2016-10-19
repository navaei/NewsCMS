using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class MessageController : Controller
    {
        //
        // GET: /Message/

        public virtual ActionResult Index(string content)
        {
            ViewBag.Message = content;
            return View();
        }

    }
}
