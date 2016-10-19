using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Mail;
using System.Net;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Robot.Helper;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Robot.Mailing
{
    public class SmtpServerConfig : SmtpClient
    {
        public SmtpServerConfig(SmtpServers server)
        {
            EnableSsl = true;
            Timeout = 300000;//10min
            DeliveryMethod = SmtpDeliveryMethod.Network;
            UseDefaultCredentials = false;
            if (server == SmtpServers.Gmail)
            {
                Host = "smtp.gmail.com";
                Port = 587;
                Credentials = new NetworkCredential("Tazeyab.com@gmail.com", "xxx");
            }
            if (server == SmtpServers.Outlook)
            {
                Host = "smtp.live.com";
                Port = 25;
                Credentials = new NetworkCredential("Tazeyab@outlook.com", "xxx");
            }
            if (server == SmtpServers.Hotmail)
            {
                Host = "smtp.live.com";
                Port = 587;
                Credentials = new NetworkCredential("Tazeyab@hotmail.com", "xxx");
            }
            if (server == SmtpServers.SendGrid)
            {
                Host = "smtp.sendgrid.net";
                Port = 587;
                Credentials = new NetworkCredential("tazeyab@tazeyab.com", "xxxx");
            }
            if (server == SmtpServers.Yahoo)
            {
                Host = "smtp.mail.yahoo.com";
                Port = 587;
                Credentials = new NetworkCredential("tazeyab@yahoo.com", "xxx");
                EnableSsl = false;
            }
            if (server == SmtpServers.Tazeyab)
            {
                Host = "smtp.tazeyab.com";
                Port = 25;
                Credentials = new NetworkCredential("noreplay@tazeyab.com", "Mn@123400");
                EnableSsl = false;
            }
            if (server == SmtpServers.ModernNet)
            {
                Host = "modernnet.org";
                Port = 25;
                Credentials = new NetworkCredential("meysam@modernnet.org", "Mn@123400");
                EnableSsl = false;
            }

        }
    }
    public class DailyMail : IStarter<StartUp>
    {

        void sendDailyEmailForAllUser()
        {
            HtmlDocument doc = new HtmlDocument();
            DailyMainContent(doc);
            GeneralLogs.WriteLog("Email sender loading...");

            TazehaContext entiti = new TazehaContext();
            var usersEmail = entiti.Users.Where(x => !x.Subscription.HasValue || x.Subscription.Value == true).Select(x => x.Email).ToList();
            GeneralLogs.WriteLogInDB("DailyEmail Ready for send...");

            #region Test

            //usersEmail.Clear();
            //usersEmail.Add("xamfia@yahoo.com");
            //usersEmail.Add("Meysam.navaei@gmail.com");
            //Helper.Utility.sendEmailByExternalServer(usersEmail.Take(40).ToList(), "پرببینده ترین های تازه یاب", doc.DocumentNode.InnerHtml, SmtpServers.Gmail);

            #endregion

            sendEmailByExternalServer("noreplay@tazeyab.com", "Tazeyab.com", usersEmail.Take(40).ToList(), "پرببینده ترین های تازه یاب", doc.DocumentNode.InnerHtml, SmtpServers.Tazeyab);
            sendEmailByExternalServer("noreplay@tazeyab.com", "Tazeyab.com", usersEmail.Skip(40).Take(80).ToList(), "پرببینده ترین های تازه یاب", doc.DocumentNode.InnerHtml, SmtpServers.Tazeyab);

            //sendEmailByExternalServer(usersEmail.Skip(80).Take(40).ToList(), "پرببینده ترین های تازه یاب", doc.DocumentNode.InnerHtml,SmtpServers.Gmail);
            //sendEmailByExternalServer(usersEmail.Skip(120).Take(40).ToList(), "پرببینده ترین های تازه یاب", doc.DocumentNode.InnerHtml, SmtpServers.Yahoo);

            //sendEmailByExternalServer(usersEmail.Skip(160).Take(40).ToList(), "پرببینده ترین های تازه یاب", doc.DocumentNode.InnerHtml,SmtpServers.Outlook);
            //sendEmailByExternalServer(usersEmail.Skip(200).Take(40).ToList(), "پرببینده ترین های تازه یاب", doc.DocumentNode.InnerHtml, SmtpServers.Outlook);

            GeneralLogs.WriteLogInDB(">OK send DailyEmail complated!!");

            Console.ReadKey();
        }
        IQueryable<FeedItem> TopItems(int TopCount)
        {
            var entiti = new TazehaContext();
            DateTime date = DateTime.Now.AddDays(-20);
            var _toItems = entiti.FeedItems.Where(x => x.CreateDate > date).OrderByDescending(x => x.VisitsCount).Take(TopCount);
            return _toItems;
        }
        IQueryable<FeedItem> MostVisitedTopItems(int TopCount)
        {
            int catDefaultId = 27;
            var entiti = new TazehaContext();
            DateTime date = DateTime.Now.AddDays(-1);
            var _toItems = entiti.FeedItems.Where(x => x.Feed.CatIdDefault == catDefaultId && x.PubDate.Value > date).OrderByDescending(x => x.VisitsCount).Take(TopCount);
            return _toItems;
        }
        List<Category> TopCategory(int TopCount)
        {
            var entiti = new TazehaContext();
            DateTime date = DateTime.Now.AddDays(-20);
            var catgroupslist = entiti.SearchHistories.Where(x => x.CreationDate < date && x.Id != null).GroupBy(x => x.Id);
            var catsid = catgroupslist.Take(TopCount).Select(x => x.Key);
            var cats = entiti.Categories.Where(x => catsid.Any(c => c == x.Id));
            return cats.ToList();
        }
        List<Tag> TopTags(int TopCount)
        {

            TazehaContext entiti = new TazehaContext();
            DateTime date = DateTime.Now.AddDays(-2);
            var taggroupslist = entiti.SearchHistories.Where(x => x.CreationDate < date && x.TagId != null).GroupBy(x => x.TagId);
            var tagsid = taggroupslist.Take(TopCount).Select(x => x.Key);
            var tags = entiti.Tags.Where(x => tagsid.Any(c => c == x.Id));
            return tags.ToList();
        }

        public void Start(StartUp inputParams)
        {
            GeneralLogs.WriteLog("Start EmailSender Daily...");
            sendDailyEmailForAllUser();
            GeneralLogs.WriteLog("End EmailSender Daily...");
        }

        public static void sendEmailByLocalServer(List<string> emailsTo, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("127.0.0.1");//"smtp.live.com"
            mail.From = new MailAddress("Info@Tazeyab.com", "خبرنامه سایت تازه یاب");
            mail.Sender = new MailAddress("Info@Tazeyab.com");
            mail.Body = body;
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 25;
            SmtpServer.Host = "www.tazeyab.com";
            //SmtpServer.EnableSsl = true;
            //SmtpServer.Timeout = 300000;//10min
            foreach (var email in emailsTo)
            {
                if (Utility.ValidEmail(email))
                {
                    mail.Bcc.Add(email);
                }
            }
            try
            {
                SmtpServer.Send(mail);
                GeneralLogs.WriteLog("OK sended Email to " + emailsTo.Count);
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog("Error sending email to " + emailsTo.Count + ex.Message);
            }


        }
        public static void sendEmailByExternalServer(string from, string fromTitle, List<string> emailsTo, string subject, string body, SmtpServers server)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpServerConfig(server);
            mail.From = new MailAddress(from, fromTitle);
            mail.Body = body;
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            foreach (var email in emailsTo)
            {
                if (Utility.ValidEmail(email))
                {
                    mail.Bcc.Add(email);
                }
            }

            try
            {
                SmtpServer.Send(mail);
                GeneralLogs.WriteLogInDB(">OK sended Email to " + emailsTo.Count);
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLogInDB(">Error sending email to " + emailsTo.Count + ex.Message);
            }

        }
        public static string sendNewsLetter(string from, string fromTitle, Dictionary<string, string> emailsAndIds, string subject, string body, SmtpServers server)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpServerConfig(server);
                mail.From = new MailAddress(from, fromTitle);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

                foreach (var email in emailsAndIds)
                {
                    if (Utility.ValidEmail(email.Value))
                    {
                        mail.Body = string.Format(body, "email=" + email.Value + "&id=" + email.Key);

                        System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(mail.Body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                        System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                        mail.AlternateViews.Add(plainView);
                        mail.AlternateViews.Add(htmlView);

                        mail.Bcc.Add(email.Value);
                        SmtpServer.Send(mail);
                        System.Threading.Thread.Sleep(1000);
                    }
                }

                var msg = "OK sended Email to " + emailsAndIds.Count;
                GeneralLogs.WriteLogInDB(msg);
                return msg;
            }
            catch (Exception ex)
            {
                var errormsg = "Error sending email to " + emailsAndIds.Count + ex.Message;
                GeneralLogs.WriteLogInDB(errormsg);
                return errormsg;
            }

        }
        public void DailyMainContent(HtmlDocument doc)
        {
            HtmlDocument xdoc = new HtmlDocument();
            //xdoc.Load(Properties.Resources.DailyMailTemplate);
            //xdoc.LoadHtml(namespace Mn.NewsCms.Robot.Properties.Resources.DailyMailTemplate);
            doc.LoadHtml(xdoc.DocumentNode.InnerHtml);
            GeneralLogs.WriteLog("Email sender load complated");
            var topItems = MostVisitedTopItems(40).ToList();
            string Content = string.Empty;
            foreach (var item in topItems)
            {
                //Content += "<li><a href=http://www.Tazeyab.com/Site/Email/" + item.FeedItemId + " target='_blank' style='color: #003562;text-decoration: none;font: 11px Tahoma;'>" + item.Title + "</a><span style='color:gray'>  / " + item.Feeds.Sites.SiteTitle.SubstringX(0, 50) + "</span><br /><hr /></li>";
                Content += "<li><a href=http://www.Tazeyab.com/Site/Email/" + item.Id + " target='_blank' style='color: #003562;text-decoration: none;font: 11px Tahoma;'>" + item.Title + "</a><br /><hr /></li>";
            }

            var node = HtmlNode.CreateNode("<div name='DItems'>" + Content + "</div>");

            var DivMostVisitedItems = doc.DocumentNode.SelectSingleNode("//td[@id='TDMostVisitedItems']");
            DivMostVisitedItems.PrependChild(node);
            //var TDMostVisitedTags = doc.DocumentNode.SelectSingleNode("//td[@id='TDMostVisitedTags']");
            //TDMostVisitedTags.PrependChild(Tagsnode);
        }
    }
    public class EmailSender
    {
        public static string sendMany(string from, string fromTitle, List<string> emailsAndIds, string subject, string body, SmtpServers server)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpServerConfig(server);
                if (string.IsNullOrEmpty(fromTitle))
                    mail.From = new MailAddress(from);
                else
                    mail.From = new MailAddress(from, fromTitle);
                mail.To.Add(from);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                //mail.Priority = MailPriority.High;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                mail.Body = body;// string.Format(body, "email=" + email.Value + "&id=" + email.Key);
                System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(mail.Body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);

                foreach (var email in emailsAndIds)
                    if (Utility.ValidEmail(email))
                        mail.Bcc.Add(email);
                mail.Bcc.Add("meysam.navaei@gmail.com");
                SmtpServer.Send(mail);
                System.Threading.Thread.Sleep(60000);

                var msg = "OK sended Email to " + emailsAndIds.Count;
                GeneralLogs.WriteLogInDB(msg);
                return msg;
            }
            catch (Exception ex)
            {
                var errormsg = "Error sending email to " + emailsAndIds.Count + ex.Message;
                GeneralLogs.WriteLogInDB(errormsg);
                return errormsg;
            }

        }
        public static string sendMany(string from, string fromTitle, List<string> emailsAndIds, string subject, string body, string smtpAddress, int port, string username, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(smtpAddress, port);
                smtpServer.Credentials = new NetworkCredential(username, password);
                smtpServer.EnableSsl = true;
                if (string.IsNullOrEmpty(fromTitle))
                    mail.From = new MailAddress(from);
                else
                    mail.From = new MailAddress(from, fromTitle);
                mail.To.Add(from);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                //mail.Priority = MailPriority.High;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                mail.Body = body;// string.Format(body, "email=" + email.Value + "&id=" + email.Key);
                System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(mail.Body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);

                foreach (var email in emailsAndIds)
                    if (Utility.ValidEmail(email))
                        mail.Bcc.Add(email);
                mail.Bcc.Add("meysam.navaei@gmail.com");
                smtpServer.Send(mail);
                System.Threading.Thread.Sleep(6000);

                var msg = "OK sended Email to " + emailsAndIds.Count;
                GeneralLogs.WriteLogInDB(msg);
                return msg;
            }
            catch (Exception ex)
            {
                var errormsg = "Error sending email to " + emailsAndIds.Count + ex.Message;
                GeneralLogs.WriteLogInDB(errormsg);
                return errormsg;
            }

        }
        public static string sendSingle(string from, string fromTitle, string emailsAddress, string subject, string body, SmtpServers server)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpServerConfig(server);
                if (string.IsNullOrEmpty(fromTitle))
                    mail.From = new MailAddress(from);
                else
                    mail.From = new MailAddress(from, fromTitle);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                //mail.Priority = MailPriority.High;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                mail.Body = body;// string.Format(body, "email=" + email.Value + "&id=" + email.Key);
                System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(mail.Body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);

                var msg = string.Empty;
                if (Utility.ValidEmail(emailsAddress))
                {
                    mail.To.Add(emailsAddress);
                    SmtpServer.Send(mail);
                    //System.Threading.Thread.Sleep(10000);
                    msg = "OK sended Email to " + emailsAddress;
                    GeneralLogs.WriteLogInDB(msg);
                }
                else
                    msg = "Error Email Address Invalid";

                return msg;
            }
            catch (Exception ex)
            {
                var errormsg = "Error sending email to " + emailsAddress + ex.Message;
                GeneralLogs.WriteLogInDB(errormsg);
                return errormsg;
            }

        }
    }

}
