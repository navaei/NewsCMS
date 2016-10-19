using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mn.NewsCms.Web.Areas.Dashboard.Models.Share
{
    public class MenuItem
    {
        public MenuItem(string title, string url)
        {
            Title = title;
            Url = url;
        }
        public string Title { get; set; }
        public string Url { get; set; }
        public string CssClass { get; set; }
    }
    public class NavigationMenu
    {

    }
}