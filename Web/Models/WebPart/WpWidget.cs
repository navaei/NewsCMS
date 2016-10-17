using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tazeyab.Web.Models
{
    public class WpWidget
    {
        public WpWidget()
        {
            HeaderColor = "#9d9d9d";
        }
        public string Title { get; set; }
        public string Code { get; set; }
        public string CssClass { get; set; }
        public string HeaderColor { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
    }
}