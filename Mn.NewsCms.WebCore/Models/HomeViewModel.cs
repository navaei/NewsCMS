using System.Collections.Generic;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.WebCore.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            CatItems = new List<HomeItemsPanelViewModel>();
            Colors = new List<string>();
            MostVisitedItems = new HomeItemsPanelViewModel();
            MostVisitedItems.Items = new List<FeedItem>();
            Pages = new List<Post>();
            Posts = new List<PostModel>();
        }
        public List<Common.Tag> TopTags { get; set; }
        public List<Post> Pages { get; set; }
        public List<SiteOnlyTitle> TopSites { get; set; }

        public List<Category> Categories { get; set; }

        public List<HomeItemsPanelViewModel> CatItems { get; set; }
        public List<PostModel> Posts { get; set; }
        public List<PostModel> Videos { get; set; }

        public HomeItemsPanelViewModel MostVisitedItems { get; set; }

        public List<string> Colors { get; set; }
    }
}