using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Common.EventsLog;
using Tazeyab.Web.Tlg;

namespace Tazeyab.Web.Controllers
{
    public partial class Tlg17Controller : Controller
    {
        // GET: Telegram
        public virtual ActionResult Test()
        {
            // var res = Ioc.ItemBiz.FeedItemsByCat("news", 8, 0, true, 20);
            //var text = "📄 جوان";
            //var today = DateTime.Now;
            ////var photo = Ioc.DataContext.PhotoItems.OrderByDescending(p => p.CreationDate).First();
            //var photo = Ioc.DataContext.PhotoItems.SingleOrDefault(x => x.Title.Replace(" ", "").StartsWith(text.Replace("📄", "").Replace(" ", "")) &&
            //  text.Replace("📄", "").Replace(" ", "").StartsWith(x.Title.Replace(" ", "")) &&
            //  x.CreationDate.Value.Year == today.Year &&
            //  x.CreationDate.Value.Month == today.Month &&
            //  x.CreationDate.Value.Day == today.Day);
            //var addss = HttpContext.Server.MapPath("~/PhotoFiles/NewsPaper/" + photo.PhotoURL);
            //var image = new Bitmap(addss);
            //MemoryStream myMemoryStream = new MemoryStream();
            //image.Save(myMemoryStream, ImageFormat.Jpeg);           
            //return base.File(image.ToByteArray(), "image/jpeg");
            Update(0, new Message()
            {
                text = "💜 اخبارایران‌وجهان",
                from = new Message_From() { username = "Mnavaei" },
                chat = new Message_Chat() { id = 121886750, username = "Mnavaei", first_name = "meysam", last_name = "navaei" }
            });
            return Content("OK");
        }
        public virtual ActionResult Messages()
        {
            var messages = string.Join("</br>", TelegramApp.Messages.OrderByDescending(m => m.CreateDateTime).Select(m => m.Name + ":" + m.Command + " ").ToList());
            return Content(messages);
        }
        public virtual ActionResult Logs()
        {
            var logs = string.Join("</br>", GeneralLogs.getLogs().OrderByDescending(l => l.CreationDate).Take(20).Select(log => log.CreationDate.ToShortTimeString() + " " + log.Code + " " + log.Value).ToList());
            return Content(logs);
        }
        public void Update(int update_id, Message message)
        {
            var res = TelegramApp.ProcessMessage(message);
            if (res)
            {
                TelegramApp.Messages.Add(new TelegramMessage()
                {
                    UpdateId = update_id,
                    ChatId = message.chat.id,
                    Command = message.text.SubstringM(0, 64).ToLower(),
                    Name = message.from.first_name + ":" + message.from.last_name + ":" + message.from.username
                });
            }
        }
    }
}