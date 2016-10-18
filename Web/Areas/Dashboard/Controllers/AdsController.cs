using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.Framework.Web.Model;
using Tazeyab.Common.Models;
using Tazeyab.Web.Areas.Dashboard.Models;
using Mn.Framework.Common.Model;
using Mn.Framework.Common;
using System.IO;
using Tazeyab.Web.Models;
using Tazeyab.Common;

namespace Tazeyab.Web.Areas.Dashboard.Controllers
{
    public partial class AdsController : BaseAdminController
    {
        // GET: Dashboard/Ads
        public virtual ActionResult Index()
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(Mn.NewsCms.Common.Resource.General.Edit, "/Dashboard/Ads/Manage/#=Id#"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resource.General.Delete, "deleteGridRow('/Dashboard/Ads/Delete/#=Id#')"));
            return View(model);
        }
        public virtual JsonResult Ads_Read([DataSourceRequest] DataSourceRequest request)
        {
            var query = Ioc.AdsBiz.GetList();
            var model = query.Select(m => new
            {
                m.Id,
                m.AdsType,
                m.Content,
                m.CreationDate,
                m.Disable,
                m.ExpireDate,
                m.Global,
                m.Link,
                m.Title,
            });
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual ActionResult Manage(int? Id)
        {
            AdsModel model;
            if (Id.HasValue)
            {
                var dbAd = Ioc.AdsBiz.GetList().SingleOrDefault(a => a.Id == Id.Value);
                model = dbAd.ToViewModel<AdsModel>();
                model.SelectedCategories = dbAd.Categories.Select(c => c.Id).ToList();
                model.SelectedTags = dbAd.Tags.Select(c => c.Id).ToList();
                ViewBag.Tags = dbAd.Tags.Select(c => new TagViewModel { Id = c.Id, Title = c.Title }).ToList();

                model.CreationDate = model.CreationDate.ToPersian();
                model.ExpireDate = model.ExpireDate.ToPersian();
            }
            else
                model = new AdsModel();

            var cats = Ioc.CatBiz.GetList().Where(c => c.Active).Select(c => new MnTitleValue() { Title = c.Title, Value = c.Id }).ToList();
            ViewBag.Categories = cats;

            //if (!model.SelectedCategories.Any())
            //    model.SelectedCategories.Add(cats.First().Value);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual JsonResult Manage(AdsModel model)
        {

            model.CreationDate = model.CreationDate.ToEnglish();
            model.ExpireDate = model.ExpireDate.ToEnglish();

            var res = new OperationStatus();
            if (ModelState.IsValid)
            {
                Ad ads;
                if (model.Id > 0)
                {
                    var dbAds = Ioc.AdsBiz.GetList().SingleOrDefault(a => a.Id == model.Id);
                    ads = model.ToModel<Ad>(dbAds);
                }
                else
                    ads = model.ToModel<Ad>();

                if (model.SelectedCategories != null)
                    ads.Categories.AddEntities(Ioc.CatBiz.GetList(model.SelectedCategories.ToList()).ToList());
                if (model.SelectedTags != null)
                    ads.Tags.AddEntities(Ioc.TagBiz.GetList().Where(t => model.SelectedTags.Contains(t.Id)).ToList());

                res = Ioc.AdsBiz.CreateEdit(ads);
            }
            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
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
                    var physicalPath = Path.Combine(Server.MapPath("~/AdsFiles"), fileName);

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
                    var physicalPath = Path.Combine(Server.MapPath("~/AdsFiles"), fileName);

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