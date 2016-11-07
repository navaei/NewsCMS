using System.Collections.Generic;
using Mn.NewsCms.Common;
using Mn.NewsCms.WebCore.Models.Shared;

namespace Mn.NewsCms.WebCore.Models
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