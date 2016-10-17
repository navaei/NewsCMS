using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.Web.Models.Shared;

namespace Tazeyab.Web.Models
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