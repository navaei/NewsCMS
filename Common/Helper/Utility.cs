using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Sockets;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using System.Text.RegularExpressions;
using System.Web.Mail;
using System.Net.Mail;
using HtmlAgilityPack;
using Mn.NewsCms.Common.Config;

namespace Mn.NewsCms.Common
{
    public class Utility
    {
        #region Web

        /// <summary>
        /// Gets or sets the time-out value in milliseconds for the System.Net.HttpWebRequest.GetResponse()
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static HtmlDocument GetXHtmlFromUri(string uri, int timeOut = 10000)
        {
            HtmlDocument htmlDoc = new HtmlDocument()
            {
                OptionCheckSyntax = true,
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Timeout = timeOut;
            //important
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.Accept = "text/html";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var stream = response.GetResponseStream())
                {
                    htmlDoc.Load(stream, Encoding.UTF8);
                }
            }
            return htmlDoc;
        }
        public static string GetWebText(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            //request.Accept = "*/*";
            try
            {
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);//, Encoding.GetEncoding("UTF-8"))
                string htmlText = reader.ReadToEnd();
                return htmlText;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string DownloadString(string address)
        {
            string strWebPage = "";
            // create request
            System.Net.WebRequest objRequest = System.Net.HttpWebRequest.Create(address);
            // get response
            System.Net.HttpWebResponse objResponse;
            objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();
            // get correct charset and encoding from the server's header
            string Charset = objResponse.CharacterSet;
            Encoding encoding = Encoding.GetEncoding(Charset);

            // read response into memory stream
            MemoryStream memoryStream;
            using (Stream responseStream = objResponse.GetResponseStream())
            {
                memoryStream = new MemoryStream();

                byte[] buffer = new byte[1024];
                int byteCount;
                do
                {
                    byteCount = responseStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, byteCount);
                } while (byteCount > 0);
            }

            // set stream position to beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader sr = new StreamReader(memoryStream, encoding);
            strWebPage = sr.ReadToEnd();

            // Check real charset meta-tag in HTML
            int CharsetStart = strWebPage.IndexOf("charset=");
            if (CharsetStart > 0)
            {
                CharsetStart += 8;
                int CharsetEnd = strWebPage.IndexOfAny(new[] { ' ', '\"', ';' }, CharsetStart);
                string RealCharset =
                       strWebPage.Substring(CharsetStart, CharsetEnd - CharsetStart);

                // real charset meta-tag in HTML differs from supplied server header???
                if (RealCharset != Charset)
                {
                    // get correct encoding
                    Encoding CorrectEncoding = Encoding.GetEncoding(RealCharset);

                    // reset stream position to beginning
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // reread response stream with the correct encoding
                    StreamReader sr2 = new StreamReader(memoryStream, Encoding.Unicode);//CorrectEncoding

                    strWebPage = sr2.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr2.Close();
                }
            }

            // dispose the first stream reader object
            sr.Close();

            return strWebPage;
        }
        public Bitmap getThumbnail(Stream StreamObj, int WitdhParam)
        {
            try
            {

                // Create a bitmap of the content of the fileUpload control in memory
                Bitmap xBMP = new Bitmap(StreamObj);

                // Calculate the new image dimensions
                int xgWidth = xBMP.Width;
                int xgHeight = xBMP.Height;
                //int sngRatio = xgWidth / xgHeight;
                int newWidth = WitdhParam;
                int newHeight = (newWidth * xgHeight) / xgWidth;

                // Create a new bitmap which will hold the previous resized bitmap
                Bitmap newBMP = new Bitmap(xBMP, newWidth, newHeight);
                // Create a graphic based on the new bitmap
                Graphics oGraphics = Graphics.FromImage(newBMP);

                // Set the properties for the new graphic file
                oGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                oGraphics.InterpolationMode = InterpolationMode.Default;
                // Draw the new graphic based on the resized bitmap
                oGraphics.DrawImage(newBMP, 0, 0, newWidth, newHeight);
                //string exten = FileName.Substring(FileName.LastIndexOf(".") + 1, FileName.Length - FileName.LastIndexOf(".") - 1);
                // Save the new graphic file to the server
                return newBMP;
                // Once finished with the bitmap objects, we deallocate them.
                xBMP.Dispose();
                newBMP.Dispose();
                oGraphics.Dispose();

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static bool UrlIsValid(string smtpHost)
        {
            bool br = false;
            try
            {
                IPHostEntry ipHost = Dns.Resolve(smtpHost);
                br = true;
            }
            catch (SocketException se)
            {
                br = false;
            }
            return br;
        }

        public static string SplitDesc(string Body, string KeyExpersion)
        {
            try
            {
                Body = Body.Trim();
                int DescLength = 500;
                if (string.IsNullOrEmpty(Body))
                    return string.Empty;
                int firstkey = !string.IsNullOrEmpty(KeyExpersion) ? Body.IndexOf(KeyExpersion) : Body.Length / 2 - 1;
                //int lastKey = Body.LastIndexOf(KeyExpersion);
                //int firstspace = Body.IndexOf(" ", firstkey);
                //int firstDot = Body.IndexOf(".", firstkey);
                if (firstkey < DescLength / 2)
                {
                    int firstspace = Body.Length > DescLength ? Body.IndexOf(" ", DescLength) : Body.Length;
                    int firstDot = Body.Length > DescLength ? Body.IndexOf(".", DescLength) : Body.Length;
                    if (firstspace == 0 && firstDot == 0)
                        return string.Empty;
                    if (firstDot != 0 && firstDot < Body.Length && firstDot > 0)
                        Body = Body.Remove(firstDot);
                    else if (firstspace < Body.Length && firstspace > 0)
                        Body = Body.Remove(firstspace);

                }
                else
                {
                    int templength = Body.IndexOf(".", firstkey) < (Body.Length - (DescLength / 2)) && Body.IndexOf(".", firstkey) > 0 ? Body.IndexOf(".", firstkey + (DescLength / 2)) : Body.Length - 1;
                    templength = templength - Body.IndexOf(".", firstkey - (DescLength / 2));
                    if (templength < DescLength / 2)
                    {
                        templength = Body.IndexOf(" ", firstkey) < Body.Length - (DescLength / 2) ? Body.IndexOf(" ", firstkey + (DescLength / 2)) : Body.Length - 1;
                        templength = templength - Body.IndexOf(" ", firstkey - (DescLength / 2));
                    }
                    int tempstart = Body.IndexOf(".", firstkey - (DescLength / 2));
                    if (tempstart <= 0)
                        tempstart = Body.IndexOf(" ", firstkey - (DescLength / 2));
                    else if (templength - tempstart < DescLength / 2)
                        tempstart = 0;
                    Body = "..." + Body.Substring(tempstart, templength - tempstart);
                }
                return Body;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string BoldKeys(string Body, string KeyExpersion)
        {
            if (string.IsNullOrEmpty(Body) || string.IsNullOrEmpty(KeyExpersion))
                return Body;
            return Body.Replace(KeyExpersion, "<Strong style='color:#350000'>" + KeyExpersion + "</Strong>");
        }

        public static string GetDateTimeHtml(DateTime? dt)
        {
            string res = string.Empty;
            try
            {
                if (dt.HasValue)
                {

                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    if (DateTime.Today.ToShortDateString() == dt.Value.ToShortDateString())
                    {
                        TimeSpan span = DateTime.Now.Subtract(dt.Value);
                        if (span.Hours > 0)
                        {
                            res = "[span  class='badge badge-warning']" + FarsiNumber(span.Hours.ToString()) + " ساعت [/span]";
                        }
                        else if (span.Minutes > 3)
                        {
                            res = "[span class='badge badge-important']" + @FarsiNumber(span.Minutes.ToString()) + " دقیقه [/span]";
                        }
                        else
                        {
                            res = "[span  class='badge badge-important']همین لحظه [/span]";
                        }
                    }
                    else if (DateTime.Today.DayOfYear - 1 == dt.Value.DayOfYear)
                    {
                        res = "[span  class='badge badge-info']دیروز [/span]";
                    }
                    else
                    {
                        res = "[span  class='badge']" + @FarsiNumber(pc.GetDayOfMonth(dt.Value).ToString() + " " + pc.GetMonthString(dt.Value).ToString()) + "[/span]";
                    }

                }

            }
            catch { }
            return res;
        }

        public static string getPersianMinDateTime(DateTime? dt)
        {
            string res = string.Empty;
            try
            {
                if (dt.HasValue)
                {

                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    if (DateTime.Today.ToShortDateString() == dt.Value.ToShortDateString())
                    {
                        TimeSpan span = DateTime.Now.Subtract(dt.Value);
                        if (span.Hours > 0)
                        {
                            res += FarsiNumber(span.Hours.ToString()) + " ساعت";
                        }
                        else if (span.Minutes > 3)
                        {
                            res = FarsiNumber(span.Minutes.ToString()) + " دقیقه";
                        }
                        else
                        {
                            res = "همین لحظه";
                        }
                    }
                    else if (DateTime.Today.DayOfYear - 1 == dt.Value.DayOfYear)
                    {
                        res = "دیروز";
                    }
                    else
                    {
                        res = FarsiNumber(pc.GetDayOfMonth(dt.Value).ToString() + " " + pc.GetMonthString(dt.Value).ToString());
                    }

                }

            }
            catch { }
            return res;
        }

        static string FarsiNumber(string str)
        {
            string s = "";
            int i;
            char[] ch = str.ToCharArray();
            foreach (char c in ch)
            {
                if (char.IsDigit(c))
                {
                    i = (int)char.GetNumericValue(c) + 1776;
                    s += ((char)i).ToString();
                }
                else
                {
                    s += c.ToString();
                }
            }
            return s;
        }

        #region IPAddress
        public static string GetClientIpAddress(HttpRequestBase request)
        {
            try
            {
                var userHostAddress = request.UserHostAddress;

                // Attempt to parse.  If it fails, we catch below and return "0.0.0.0"
                // Could use TryParse instead, but I wanted to catch all exceptions
                IPAddress.Parse(userHostAddress);

                var xForwardedFor = request.ServerVariables["X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(xForwardedFor))
                    return userHostAddress;

                // Get a list of public ip addresses in the X_FORWARDED_FOR variable
                var publicForwardingIps = xForwardedFor.Split(',').Where(ip => !IsPrivateIpAddress(ip)).ToList();

                // If we found any, return the last one, otherwise return the user host address
                return publicForwardingIps.Any() ? publicForwardingIps.Last() : userHostAddress;
            }
            catch (Exception)
            {
                // Always return all zeroes for any failure (my calling code expects it)
                return "0.0.0.0";
            }
        }

        private static bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }

        public static bool IsIpFromIran(HttpRequestBase request)
        {
            string res = GetClientIpAddress(request);
            var ip = Double.Parse(res);
            var fromIp = Double.Parse("773605376");
            var toIp = Double.Parse("773607423");
            if (ip >= fromIp && ip <= toIp)
            {
                return true;
            }
            return false;
        }
        #endregion
        #endregion
        #region Robot
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
        public static bool ValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }
        public static void FileUploadToFtp(FileInfo file, string VirtualPath, string ftphost, string ftpUserName, string ftpPassword)
        {
            var ftpfullpath = "ftp://" + ftphost + (VirtualPath[0] == '/' ? VirtualPath : "/" + VirtualPath) + file.Name;
            var ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
            ftp.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
            //userid and password for the ftp server to given  

            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            var fs = file.OpenRead(); // File.OpenRead(inputfilepath);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            var ftpstream = ftp.GetRequestStream();
            ftpstream.Write(buffer, 0, buffer.Length);
            ftpstream.Close();
        }
        #endregion

    }
}