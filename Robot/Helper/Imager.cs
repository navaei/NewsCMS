using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Net;
using Tazeyab.Common;
using System.Drawing.Imaging;
using System.Web;
using Tazeyab.Common.Models;
using Tazeyab.CrawlerEngine.Helper;
using Tazeyab.Common.EventsLog;

namespace CrawlerEngine.Helper
{
    public class Imager
    {
        public static Bitmap GetThumbnail(Stream StreamObj, int WitdhParam)
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
        public static bool SaveThumbnail(Stream StreamObj, string path, string FileName, int ThumbnailWidth)
        {
            // Create a bitmap of the content of the fileUpload control in memory
            Bitmap xBMP = new Bitmap(StreamObj);

            // Calculate the new image dimensions
            int xgWidth = xBMP.Width;
            int xgHeight = xBMP.Height;
            //int sngRatio = xgWidth / xgHeight;
            int newWidth = ThumbnailWidth;
            int newHeight = (newWidth * xgHeight) / xgWidth;

            // Create a new bitmap which will hold the previous resized bitmap
            Bitmap newBMP = new Bitmap(xBMP, newWidth, newHeight);
            // Create a graphic based on the new bitmap
            Graphics oGraphics = Graphics.FromImage(newBMP);

            // Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(newBMP, 0, 0, newWidth, newHeight);
            //string exten = FileName.Substring(FileName.LastIndexOf(".") + 1, FileName.Length - FileName.LastIndexOf(".") - 1);
            // Save the new graphic file to the server              
            //xBMP.Dispose();
            //oGraphics.Dispose();
            newBMP.SetResolution(20, 20);
            return SaveImage(newBMP, path, FileName);

        }
        public static bool SaveImage(Stream StreamObj, string path, string FileName)
        {
            // Create a bitmap of the content of the fileUpload control in memory
            Bitmap xBMP = new Bitmap(StreamObj);
            return SaveImage(xBMP, path, FileName);
        }
        public static bool SaveImage(Bitmap bitmap, string Virtualpath, string fileName)
        {
            try
            {

                string fullpath = AppDomain.CurrentDomain.BaseDirectory + Virtualpath; ;
                if (!Directory.Exists(fullpath))
                    Directory.CreateDirectory(fullpath);
                bitmap.Save(fullpath + fileName);
                return true;
                //}
            }
            catch (Exception ex)
            {
                Tazeyab.Common.EventsLog.GeneralLogs.WriteLog("NewsPaper Save Image " + ex.Message, TypeOfLog.Error);
                return false;
            }

        }
        public static bool SaveImage(string url, string destinationPath)
        {
            var stream = Requester.GetStreamFromUri(url, "");
            var bitmap = new Bitmap(stream);
            if (bitmap.Width > 3000 || bitmap.Width < bitmap.Height)
                return false;

            bitmap.Save(destinationPath + ".jpg", ImageFormat.Jpeg);
            return true;

        }
        public static Bitmap ResizeBitmap(Bitmap originalBitmap, int requiredHeight, int requiredWidth)
        {
            int[] heightWidthRequiredDimensions;

            // Pass dimensions to worker method depending on image type required
            heightWidthRequiredDimensions = WorkDimensions(originalBitmap.Height, originalBitmap.Width, requiredHeight, requiredWidth);


            Bitmap resizedBitmap = new Bitmap(heightWidthRequiredDimensions[1],
                                               heightWidthRequiredDimensions[0]);

            const float resolution = 30;

            resizedBitmap.SetResolution(resolution, resolution);

            Graphics graphic = Graphics.FromImage((Image)resizedBitmap);

            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

            graphic.Dispose();
            originalBitmap.Dispose();
            //resizedBitmap.Dispose(); // Still in use


            return resizedBitmap;
        }
        private static int[] WorkDimensions(int originalHeight, int originalWidth, int requiredHeight, int requiredWidth)
        {
            int imgHeight = 0;
            int imgWidth = 0;

            imgWidth = requiredHeight;
            imgHeight = requiredWidth;


            int requiredHeightLocal = originalHeight;
            int requiredWidthLocal = originalWidth;

            double ratio = 0;

            // Check height first
            // If original height exceeds maximum, get new height and work ratio.
            if (originalHeight > imgHeight)
            {
                ratio = double.Parse(((double)imgHeight / (double)originalHeight).ToString());
                requiredHeightLocal = imgHeight;
                requiredWidthLocal = (int)((decimal)originalWidth * (decimal)ratio);
            }

            // Check width second. It will most likely have been sized down enough
            // in the previous if statement. If not, change both dimensions here by width.
            // If new width exceeds maximum, get new width and height ratio.
            if (requiredWidthLocal >= imgWidth)
            {
                ratio = double.Parse(((double)imgWidth / (double)originalWidth).ToString());
                requiredWidthLocal = imgWidth;
                requiredHeightLocal = (int)((double)originalHeight * (double)ratio);
            }

            int[] heightWidthDimensionArr = { requiredHeightLocal, requiredWidthLocal };

            return heightWidthDimensionArr;
        }
        private static Image getImageFromBytes(byte[] myByteArray)
        {
            System.IO.MemoryStream newImageStream = new System.IO.MemoryStream(myByteArray, 0, myByteArray.Length);
            Image image = Image.FromStream(newImageStream, true);
            Bitmap resized = new Bitmap(image, image.Width / 2, image.Height / 2);
            image.Dispose();
            newImageStream.Dispose();
            return resized;
        }
        public static Bitmap getImageFromBStream(Stream newImageStream)
        {
            try
            {
                Image image = Image.FromStream(newImageStream, true);
                Bitmap resized = new Bitmap(image, image.Width / 2, image.Height / 2);
                //image.Dispose();
                //newImageStream.Dispose();
                return resized;
            }
            catch
            {
                return null;
            }
        }
        //private void saveFileOnFtp(Bitmap bitmap, string path)
        //{
        //    string filename = Server.MapPath("file1.txt");
        //    string ftpServerIP = "ftp.Tazeyab.com/";
        //    string ftpUserName = Config.getConfig<string>("FtpUserName");
        //    string ftpPassword = Config.getConfig<string>("FtpPassword");

        //    FileInfo objFile = new FileInfo(filename);
        //    FtpWebRequest objFTPRequest;

        //    // Create FtpWebRequest object 
        //    objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + objFile.Name));

        //    // Set Credintials
        //    objFTPRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

        //    // By default KeepAlive is true, where the control connection is 
        //    // not closed after a command is executed.
        //    objFTPRequest.KeepAlive = false;

        //    // Set the data transfer type.
        //    objFTPRequest.UseBinary = true;

        //    // Set content length
        //    objFTPRequest.ContentLength = objFile.Length;

        //    // Set request method
        //    objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

        //    // Set buffer size
        //    int intBufferLength = 16 * 1024;
        //    byte[] objBuffer = new byte[intBufferLength];

        //    // Opens a file to read
        //    FileStream objFileStream = objFile.OpenRead();

        //    try
        //    {
        //        // Get Stream of the file
        //        Stream objStream = objFTPRequest.GetRequestStream();

        //        int len = 0;

        //        while ((len = objFileStream.Read(objBuffer, 0, intBufferLength)) != 0)
        //        {
        //            // Write file Content 
        //            objStream.Write(objBuffer, 0, len);

        //        }

        //        objStream.Close();
        //        objFileStream.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
