using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common.Models;

namespace Tazeyab.DomainClasses.ContentManagment
{
    public class Remoting
    {
        public static void InsertRemoteRequestLogs(string Controller, string Content)
        {
            try
            {
                TazehaContext context = new TazehaContext();
                string reqref = HttpContext.Current.Request.UrlReferrer.ToString();
                context.RemoteRequestLogs.Add(new RemoteRequestLog { RequestRefer = reqref, CreationDate = DateTime.Now, Controller = Controller, Content = Content });
                context.SaveChanges();
            }
            catch { }
        }
    }
}