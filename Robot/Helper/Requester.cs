using System;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using Mn.NewsCms.Common.EventsLog;

namespace Mn.NewsCms.Robot.Helper
{
    public class Requester
    {
        const string UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
        const int RequestTimeOut = 5000;

        public byte[] GetByteFromUri(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.UserAgent = UserAgent;
            request.Timeout = RequestTimeOut;
            try
            {
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                byte[] arr = memoryStream.ToArray();
                return arr;
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog("GetByteFromUri" + ex.Message.SubstringM(0, 256), Common.Models.TypeOfLog.Error);
                return null;
            }
        }
        public static Stream GetStreamFromUri(string uri, string RequestAccept)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.UserAgent = UserAgent;
            request.Timeout = RequestTimeOut;
            if (!string.IsNullOrEmpty(RequestAccept))
                request.Accept = RequestAccept;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            try
            {
                var response = request.GetResponse() as HttpWebResponse;
                var stream = response.GetResponseStream();
                return stream;
            }
            catch
            {
                return null;
            }
        }
        public static HtmlDocument GetXHtmlFromUri(string uri)
        {
            HtmlDocument htmlDoc = new HtmlDocument()
            {
                OptionCheckSyntax = true,
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            var content = GetStreamFromUri(uri, "text/html");
            if (content != null)
                htmlDoc.Load(content, Encoding.UTF8);
            return htmlDoc;
        }
        public static string GetFileNameFromURL(string url)
        {
            Uri uri = new Uri(url);
            string filename = Path.GetFileName(uri.LocalPath);
            return filename;
        }
        /// <summary>
        /// Gets the response text for a given url.
        /// </summary>
        /// <param name="url">The url whose text needs to be fetched.</param>
        /// <returns>The text of the response.</returns>
        public static string GetWebText(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = UserAgent;
            request.Timeout = RequestTimeOut;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            //try
            //{
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string htmlText = reader.ReadToEnd();
            return htmlText;
            //}
            //catch (Exception ex)
            //{
            //    GeneralLogs.WriteLogInDB("Error GetWebText " + ex.Message, Common.Models.TypeOfLog.Error, typeof(Requester));
            //    return string.Empty;
            //}
        }

        public static Page GetPage(string url)
        {
            return new Page(GetWebText(url));
        }
    }
}
