using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Web.WebLogic.Binder;

namespace Tazeyab.Web.Areas.Dashboard.Models
{
    public class RwpModel : RemoteWebPart
    {

        [Display(Name = "دسته بندی")]
        public List<long> SelectedCategories { get; set; }

        [Display(Name = "تگ های مرتبط")]
        public List<long> SelectedTags { get; set; }
    }
}