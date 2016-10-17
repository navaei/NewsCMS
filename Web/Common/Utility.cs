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

namespace Tazeyab.Web.BLL
{
    public class Utility
    {
        public static string GetWebText(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = "PersianFeedCrawler";
            try
            {
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
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
    }
}