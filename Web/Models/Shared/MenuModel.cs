using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Navigation;

namespace Mn.NewsCms.Web.Models
{
    public class MenuModel
    {
        public List<Category> Categories { get; set; }
        public Menu Menu { get; set; }
    }
}