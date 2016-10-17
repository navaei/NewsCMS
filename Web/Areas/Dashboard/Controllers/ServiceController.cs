using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Web.Models;

namespace Tazeyab.Web.Areas.Dashboard.Controllers
{
    public partial class ServiceController : BaseAdminController
    {
        public virtual ActionResult Index()
        {
            return View();
        }      
    }
}