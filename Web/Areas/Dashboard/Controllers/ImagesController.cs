using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tazeyab.Web.Areas.Dashboard.Controllers
{
    public partial class ImageBrowserController : EditorImageBrowserController
    {
        private const string contentFolderRoot = "~/Uploads/";
        private const string prettyName = "Images/";
        private static readonly string[] foldersToCopy = new[] { "~/Uploads/Shared/" };
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

        private const int DefWidth = 500;
        private const int ThumbnailHeight = 130;
        private const int ThumbnailWidth = 180;

        private DirectoryBrowser directoryBrowser;
        private ThumbnailCreator thumbnailCreator;

        public ImageBrowserController()
        {
            directoryBrowser = new DirectoryBrowser();
            thumbnailCreator = new ThumbnailCreator(new FitImageResizer());
        }

        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string ContentPath
        {
            get
            {
                return Path.Combine(contentFolderRoot, prettyName);
                // CreateUserFolder();
            }
        }

        private string CreateUserFolder()
        {
            var virtualPath = Path.Combine(contentFolderRoot, DateTime.Now.Year.ToString() + "/", prettyName);

            var path = Server.MapPath(virtualPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (var sourceFolder in foldersToCopy)
                {
                    CopyFolder(Server.MapPath(sourceFolder), path);
                }
            }
            return virtualPath;
        }

        private void CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (var file in Directory.EnumerateFiles(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(file));
                System.IO.File.Copy(file, dest);
            }

            foreach (var folder in Directory.EnumerateDirectories(source))
            {
                var dest = Path.Combine(destination, Path.GetFileName(folder));
                CopyFolder(folder, dest);
            }
        }

        public virtual ActionResult UploadThumb(string path, HttpPostedFileBase file)
        {
            if (!string.IsNullOrEmpty(path))
                path = path[path.Length - 1] == '/' ? path : path + '/';

            using (Image img = Image.FromStream(file.InputStream))
            {
                Bitmap bitmap = new Bitmap(img);
                if (bitmap.Size.Width > ThumbnailWidth || bitmap.Size.Height > ThumbnailHeight)
                {
                    //using (Graphics graphics = Graphics.FromImage(bitmap))
                    //{
                    //    //set the resize quality modes to high quality
                    //    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //    //draw the image into the target bitmap
                    //    graphics.DrawImage(img, 0, 0, ThumbnailWidth, ThumbnailHeight);
                    //}
                    var newHeight = bitmap.Size.Height / bitmap.Size.Width * DefWidth;
                    var bitmapNormal = new Bitmap(img, DefWidth, newHeight);
                    var bitmap8080 = new Bitmap(img, ThumbnailWidth, ThumbnailHeight);

                    bitmapNormal.Save(Server.MapPath(ContentPath + path + file.FileName), ImageFormat.Jpeg);
                    bitmap8080.Save(Server.MapPath(ContentPath + path + file.FileName + "_80x80"), ImageFormat.Jpeg);
                }
                else
                    bitmap.Save(Server.MapPath(ContentPath + path + file.FileName), ImageFormat.Jpeg);


            }
            //var res = base.Upload(path, file);
            return Json(new
                       {
                           name = Path.GetFileName(file.FileName)
                       }, "text/plain");
            //return View(new { Data = file.FileName });
        }
        public virtual ActionResult CreateThumb(string path, FileBrowserEntry entry)
        {
            return View();
            return CreateThumbnail(path);
        }
        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (var fileStream = System.IO.File.OpenRead(physicalPath))
            {
                var desiredSize = new ImageSize
                {
                    Width = ThumbnailWidth,
                    Height = ThumbnailHeight
                };

                const string contentType = "image/jpeg";

                return File(thumbnailCreator.Create(fileStream, desiredSize, contentType), contentType);
            }
        }
    }
}