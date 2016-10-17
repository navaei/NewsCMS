using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Mail;
using CrawlerEngine;
using Tazeyab.Common.Models;
using Divan;
using System.Web.Script.Serialization;
using Tazeyab.CrawlerEngine.EmailSender;
using Tazeyab.Common.EventsLog;
using System.Net;
using Tazeyab.Common;
using System.IO;

namespace Tazeyab.CrawlerEngine.Helper
{
    public class Utility
    {
        public static bool HasFaWord(String word)
        {
            if (string.IsNullOrEmpty(word))
                return false;
            foreach (var item in word)
            {
                if ((int)item > 1550 && (int)item < 1680)
                    return true;
            }
            return false;
        }
        public static string getOGImage(string PageHtml)
        {
            try
            {
                foreach (Match match in Regex.Matches(PageHtml, "<meta property=\"og:image\" content=\"(.*?)\"/>"))
                {
                    return match.Groups[1].Value;
                }
                foreach (Match match in Regex.Matches(PageHtml, "<meta content=\"(.*?)\" property=\"og:image\" />"))
                {
                    return match.Groups[1].Value;
                }
            }
            catch { }
            return string.Empty;
        }
        private static void AddRangeButNoDuplicates(List<string> targetList, List<string> sourceList)
        {
            foreach (string str in sourceList)
            {

                if (!targetList.Contains(str.ToUpper()))
                    targetList.AddX(str.ToUpper());
            }
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
                if (ValidEmail(email))
                {
                    mail.Bcc.Add(email);
                }
            }
            try
            {
                SmtpServer.Send(mail);
                GeneralLogs.WriteLog(">OK sended Email to " + emailsTo.Count);
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog(">Error sending email to " + emailsTo.Count + ex.Message);
            }


        }
        public static void sendEmailByExternalServer(List<string> emailsTo, string subject, string body, SmtpServers server)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpServerConfig(server);
            mail.From = new MailAddress("Info@Tazeyab.com", "خبرنامه تازه یاب");
            mail.Body = body;
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            foreach (var email in emailsTo)
            {
                if (ValidEmail(email))
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
        public static bool ValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }
        public static void InsertItemToCloudant(FeedItemSP item)
        {
            CouchServer server = new CouchServer("xamfia.cloudant.com", 5984, "xamfia", "123400");
            ICouchDatabase database = server.GetDatabase("docs");
            //var docs = database.GetAllDocuments();
            //string doc = "{'title':'test','tags':'mn,jv'}";
            database.WriteDocument(ItemToJson(item), item.FeedItemId.ToString());
        }
        private static string ItemToJson(FeedItemSP item)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string JsonData = ser.Serialize(new { item.FeedItemId, item.Title, item.Description, item.SiteTitle, item.SiteId, item.Link, item.PubDate, item.Cats });
            return JsonData;

            string cats = String.Join(",", item.Cats.ToArray());
            cats = string.IsNullOrEmpty(cats) ? "00" : cats;
            string json = string.Format(@"{ 'FeedItemId':'{0}','Title' : '{1}' ,'Description':'{2}','SiteTitle' : '{3}' ",
                item.FeedItemId, item.Title, item.Description, item.SiteTitle);
            json += string.Format(" 'SiteId':'{0}' , 'Link':'{1}' ,'PubDate' : '{2}' , 'Cats':'{3}'  }", item.SiteId, item.Link, item.PubDate.Value.ToString("yyyyMMddHH"), cats);
            return json;
        }
        public static void FileUploadToFtp(FileInfo file, string VirtualPath)
        {
            string ftphost = "tazeyab.com";

            string ftpfullpath = "ftp://" + ftphost + (VirtualPath[0] == '/' ? VirtualPath : "/" + VirtualPath) + file.Name;
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
            ftp.Credentials = new NetworkCredential(Config.getConfig<string>("FtpUserName"), Config.getConfig<string>("FtpPassword"));
            //userid and password for the ftp server to given  

            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            FileStream fs = file.OpenRead(); // File.OpenRead(inputfilepath);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            Stream ftpstream = ftp.GetRequestStream();
            ftpstream.Write(buffer, 0, buffer.Length);
            ftpstream.Close();
        }
    }
}
