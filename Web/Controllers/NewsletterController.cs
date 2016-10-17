using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common.Models;

namespace Tazeyab.Web.Controllers
{
    public partial class NewsletterController : Controller
    {
        //
        // GET: /Newsletter/

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult subscriptDailyNews(string email, int type)
        {
            //using (var context = new TazehaContext())
            //{
            //    if (type == 1)
            //        context.NewsletterUsers.Add(new NewsletterUser() { NewsletterUserID = Guid.NewGuid(), Email = email, DailyNewsletter = true, DailyNewsPaper = false });
            //    else if (type == 2)
            //        context.NewsletterUsers.Add(new NewsletterUser() { NewsletterUserID = Guid.NewGuid(), Email = email, DailyNewsletter = false, DailyNewsPaper = true });
            //    else if (type == 3)
            //        context.NewsletterUsers.Add(new NewsletterUser() { NewsletterUserID = Guid.NewGuid(), Email = email, DailyNewsletter = true, DailyNewsPaper = true });
            //    try
            //    {
            //        context.SaveChanges();
            //        return RedirectToAction(MVC.Message.ActionNames.Index, MVC.Message.Name, new { Content = "عضویت شما در خبرنامه با موفقیت انجام شد" });
            //    }
            //    catch (Exception ex)
            //    {
            //        return RedirectToAction(MVC.Message.ActionNames.Index, MVC.Message.Name, new { Content = "خطا در عضویت در خبرنامه" });
            //    }
            //}

            return RedirectToAction(MVC.Message.ActionNames.Index, MVC.Message.Name, new { Content = "خطا در عضویت در خبرنامه" });
        }

        #region Unsubscript
        public virtual ActionResult UnsubscriptDailyNews(string id, string email)
        {
            //TazehaContext context = new TazehaContext();
            //var guid = Guid.Parse(id);
            //var newsletterUser = context.NewsletterUsers.SingleOrDefault(x => x.NewsletterUserID == guid && x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
            //if (newsletterUser != null)
            //{
            //    context.NewsletterUsers.Remove(newsletterUser);
            //    context.SaveChanges();
            //}
            //else
            //    return RedirectToAction(MVC.Message.ActionNames.Index, MVC.Message.Name, new { Content = "ایمیل ارسالی شما در سیستم ثبت نشده است." });

            return RedirectToAction(MVC.Message.ActionNames.Index, MVC.Message.Name, new { Content = "نام کاربری شما از لیست ایمیل های روزانه حذف شد و دیگر ایمیلی برایتان ارسال نخواهد شد" });
        }

        #endregion
    }
}
