using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.Areas.Dashboard.Models;
using System.IO;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class TagsController : BaseAdminController
    {
        private readonly ITagBusiness _tagBusiness;

        public TagsController(ITagBusiness tagBusiness)
        {
            _tagBusiness = tagBusiness;
        }

        public virtual ActionResult Index()
        {
            ViewBag.Tags = _tagBusiness.GetList().ToList().Select(t => t.ToViewModel<TagViewModel>()).ToList();//.Select(t => new { t.Id, t.Title })
            return View();
        }
        public virtual ActionResult HotKey()
        {
            return View();
        }
        public virtual JsonResult CreateEditHotKey(RecentKeyWord key)
        {
            OperationStatus res;
            if (key.Id == 0)
                res = _tagBusiness.CreateHotKey(key);
            else
                res = _tagBusiness.UpdateHotKey(key);

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult DeleteHotKey(RecentKeyWord key)
        {
            var res = _tagBusiness.DeleteHotKey(key);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonNetResult HotTags_Read([DataSourceRequest] DataSourceRequest request)
        {
            var query _tagBusiness.GetListRecentKeyWord().Select(t => new { t.Id, t.Title, t.Value, t.IsTag }).ToList();
            return JsonNet(query.ToDataSourceResult(request));
        }
        public virtual JsonNetResult Tag_Read([DataSourceRequest] DataSourceRequest request, long? feedId)
        {
            if (request.PageSize == 0)
                request.PageSize = 10;

            var query = _tagBusiness.GetList().Select(t => new TagViewModel()
            {
                BackgroundColor = t.BackgroundColor,
                ImageThumbnail = t.ImageThumbnail,
                Color = t.Color,
                EnValue = t.EnValue,
                Id = t.Id,
                InIndex = t.InIndex,
                ParentTagId = t.ParentTagId,
                Value = t.Value,
                Title = t.Title
            });

            return JsonNet(query.ToDataSourceResult(request));
        }

        public virtual JsonNetResult Tags_Data_Read(string term, bool isGrid = true)
        {
            var query = _tagBusiness.GetList().Where(t => string.IsNullOrEmpty(term) || t.Title.StartsWith(term) || t.EnValue.StartsWith(term)).Select(t => new TagViewModel()
            {
                BackgroundColor = t.BackgroundColor,
                ImageThumbnail = t.ImageThumbnail,
                Color = t.Color,
                EnValue = t.EnValue,
                Id = t.Id,
                InIndex = t.InIndex,
                ParentTagId = t.ParentTagId,
                Title = t.Title
            }).Take(10).ToList();

            return JsonNet(query);
        }

        public virtual ActionResult Manage()
        {
            return View();
        }
        public virtual JsonResult CreateEdit(TagViewModel tag)
        {
            var poco = _tagBusiness.Get(tag.Id);
            poco = tag.ToModel<Tag>(poco);
            var res = _tagBusiness.Update(poco);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult Delete(TagViewModel key)
        {
            var res = _tagBusiness.Delete(key.Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Images/LogicalImage"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public virtual ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/~/Images/LogicalImage"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}