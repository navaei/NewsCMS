using System.Collections.Generic;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.WebCore.Models.Tag
{
    public class TagItemsPageModel
    {
        public TagItemsPageModel()
        {
            Posts = new List<Post>();
            VisualItems = new List<FeedItem>();
            Items = new List<FeedItem>();
        }
        public List<Post> Posts { get; set; }
        public List<FeedItem> VisualItems { get; set; }
        public List<FeedItem> Items { get; set; }
    }
}