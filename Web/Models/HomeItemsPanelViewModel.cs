using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Web.Models.Shared;

namespace Mn.NewsCms.Web.Models
{
    public class HomeItemsPanelViewModel
    {
        public HomeItemsPanelViewModel()
        {
            ShowMoreBtn = true;
            CssClass = string.Empty;
        }
        public SliderModel slider { get; set; }
        public List<FeedItem> Items { get; set; }
        public Category Cat { get; set; }
        public string CssClass { get; set; }
        public bool ShowMoreBtn { get; set; }
    }
}