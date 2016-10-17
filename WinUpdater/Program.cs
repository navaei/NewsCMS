using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common;
using Tazeyab.CrawlerEngine;
using Tazeyab.CrawlerEngine.Updater;
using System.Web;


namespace Tazeyab.WinUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var query = @"آگهی-استخدام-گروه-پژوهشی-فن‌آوری-اطلاعات-جهاد-دانشگاهی-شریف";
            var res = System.Web.HttpUtility.HtmlEncode(query);
            var res2 = System.Net.WebUtility.UrlEncode(query);
            //IBaseServer baseservice = new WinBaseServer();
            //var updater = new FeedUpdater(baseservice);
            //updater.AutoUpdater();
            //updater.AutoUpdater();
        }
    }
}
