using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Navigation;

namespace Tazeyab.Web.Models
{
    public class MenuModel
    {
        public List<Category> Categories { get; set; }
        public Menu Menu { get; set; }
    }
}