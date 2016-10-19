using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class Report
    {
        public string TopAnalyseReport()
        {
            TazehaContext context = new TazehaContext();
            string str = "";            
            var itemscount = context.FeedItems.Count();
            var sitecount = context.Sites.Count();
            str = string.Format("تعداد سایت های یافته شده {0} {1} مجموع مطالب موجود {2}  ", sitecount, " و ", itemscount);
            return str;
        }
    }
}