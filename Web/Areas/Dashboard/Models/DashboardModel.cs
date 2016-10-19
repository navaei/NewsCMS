using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Membership;

namespace Mn.NewsCms.Web.Areas.Dashboard.Models
{
    public class DashboardModel
    {
        public int ActiveFeedsCount { get; set; }
        public int SitesCount { get; set; }
        public int MessagesCount { get; set; }
        public int UsersCount { get; set; }
        public int PostsCount { get; set; }
        public int TodayFeedsCount { get; set; }
        public int TodayItemsCount { get; set; }
        public int TodayCommentsCount { get; set; }
        public List<Comment> Comments { get; set; }
        public List<User> Users { get; set; }
        public List<Feed> Feeds { get; set; }
        public List<Post> Posts { get; set; }
    }
}