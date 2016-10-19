using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class FileBrowserController : EditorFileBrowserController
    {

        // GET: Dashboard/FileBrowser       
        private const string contentFolderRoot = "~/Uploads/Files/";
        public override string ContentPath
        {
            get { return contentFolderRoot; }
        }
    }
}