using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;

namespace Mn.NewsCms.Robot.SiteImprovement
{
    public class SiteMap
    {
        public static string GenerateTemp()
        {
            #region Property
            int countOfCat = 0; int CountOfTag = 0; int CountOfSite = 0;
            var context = new TazehaContext();
           var changefreqs = new Dictionary<long, string>();
            changefreqs.Add(95, "hourly");
            changefreqs.Add(100, "hourly");
            changefreqs.Add(105, "daily");
            changefreqs.Add(110, "weekly");
            changefreqs.Add(115, "monthly");
            changefreqs.Add(120, "yearly");
            changefreqs.Add(200, "never");

            TextWriter tw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~\\sitemap.xml"));
            string res = string.Empty;
            string url4params = @"<url>
             <loc>http://www.tazeyab.com/{0}/{1}/</loc>
             <lastmod>2013-06-20T15:47:10+00:00</lastmod>
             <changefreq>{2}</changefreq>
             {3}
             </url>";
            string url3params = @"<url>
             <loc>http://www.tazeyab.com/{0}/{1}/</loc>
             <lastmod>2013-06-20T15:47:10+00:00</lastmod>
             <changefreq>{2}</changefreq>             
             </url>";
            #endregion

            tw.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
            tw.WriteLine("<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>");

            foreach (var cat in context.Categories.Where(x => x.ViewMode != ViewMode.NotShow))
            {
                tw.WriteLine(string.Format(url4params, "cat", string.IsNullOrEmpty(cat.Code) ? cat.Title.ToLower() : cat.Code.ToLower(), "always", "<priority>0.8</priority>"));
                countOfCat++;
            }
            foreach (var tag in context.Tags)
            {
                tw.WriteLine(string.Format(url4params, "tag", string.IsNullOrEmpty(tag.EnValue) ? tag.Value.ToLower() : tag.EnValue.ToLower(), "hourly", "<priority>0.7</priority>"));
                CountOfTag++;
            }

            var feeds = (from f in context.Feeds
                         join s in context.Sites on f.Id equals s.Id
                         where s.PageRank > 2 && f.UpdateDurationId.HasValue && f.UpdateDurationId.Value < 115
                         select new { s.SiteUrl, f.UpdateDurationId })
                         .OrderBy(o => o.UpdateDurationId)
                         .GroupBy(g => new { g.SiteUrl })
                         .Select(s => s.FirstOrDefault()).ToList();

            foreach (var feed in feeds)
            {
                tw.WriteLine(string.Format(url3params, "site", feed.SiteUrl, feed.UpdateDurationId.HasValue ? changefreqs[feed.UpdateDurationId.Value] : "never", string.Empty));
                CountOfSite++;
            }


            tw.WriteLine("</urlset>");
            tw.Close();
            return string.Format("SuccessFully generate sitemap.[ br /] Number of Cat:{0} Tag:{1} Site:{2}", countOfCat, CountOfTag, CountOfSite);
        }
    }
}
