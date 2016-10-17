using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common.ExternalService;

namespace Mn.Components.Telegram
{
    public delegate void MessageSent(message msg);

    public class Program : ITelegramService
    {
        List<Command> Commands { get; set; }
        public string Token { get; set; }
        public event MessageSent Sent = delegate { };
        public void GetUpdates()
        {
            WebRequest req = WebRequest.Create("https://api.telegram.org/bot" + Token + "/getUpdates");
            req.UseDefaultCredentials = true;
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string s = sr.ReadToEnd();
            sr.Close();
            var jobject = JObject.Parse(s);
            mydata gg = JsonConvert.DeserializeObject<mydata>(jobject.ToString());
            List<result> results = new List<result>();
            foreach (result rs in gg.result)
            {
                if (rs.message.text)
                    results.Add(rs);
                SendMessage(rs.message.chat.id.ToString(), "hello" + " " + "Dear" + rs.message.chat.first_name);
                Sent(rs.message);
            }
        }
        public void ProcessMessage(message message)
        {
            var comand = Commands.SingleOrDefault(c => c.Title.StartsWith(message.text) || c.Code == message.text.Trim());
            if (comand != null)
            {

            }
            else
            {

            }

        }
        public void SendMessage(string chat_id, string message)
        {
            WebRequest req = WebRequest.Create("https://api.telegram.org/bot" + Token + "/sendMessage?chat_id=" + chat_id + "&text=" + message);
            req.UseDefaultCredentials = true;

            var result = req.GetResponse();
            req.Abort();
        }

        public void StartApp()
        {
            Commands = new List<Command>();
            var start = new Command()
            {
                Title = "منوی اصلی",
                Code = "start",
                CommandType = CommandType.Custom,
                Commands = string.Format("[[\"{0}\",\"{1}\"],[\"{2}\",\"{3}\",\"{4}\"],[\"{5}\",\"{6}\"]]", "اخبارایران‌وجهان", "تازه‌های فوتبالی", "دنیای فناوری", "سرگرمی", "استخدام", "پربیننده ترین", "سایتهای خبری")
            };
            var news = new Command()
            {
                Title = "اخبارایران‌وجهان",
                Code = "news",
                CommandType = CommandType.Category,
            };
            var football = new Command()
            {
                Title = "تازه‌های فوتبالی ",
                Code = "football",
                CommandType = CommandType.Category,
                Commands = string.Format("[[\"{0}\",\"{1}\",\"{2}\"],[\"{3}\",\"{4}\"],[\"{5}\",\"{6}\",\"{7}\"]]", "پرسپولیس", "استقلال", "لیگ برتر", "رئال مادرید", "بارسلونا", "منچستر", "یوونتوس", "تیم های بیشتر")
            };
            var moreTeam = new Command()
            {
                Title = "تیم های بیشتر",
                Code = "moreteam",
                CommandType = CommandType.Custom,
                Commands = string.Format("[[\"{0}\",\"{1}\",\"{2}\"],[\"{3}\",\"{4}\"],[\"{5}\",\"{6}\",\"{7}\"]]", "سپاهان", "تراکتور", "لیگ برتر", "اینترمیلان", "آث میلان", "آرسنال", "چلسی", "بایرن")
            };
            var news = new Command()
           {
               Title = "دنیای فناوری",
               Code = "it",
               CommandType = CommandType.Category,
           };
            var news = new Command()
           {
               Title = "سرگرمی",
               Code = "fun",
               CommandType = CommandType.Category,
               Commands = string.Format("[[\"{0}\",\"{1}\",\"{2}\"]]", "سینما", "دانلود بازی", "دانلود فیلم")
           };
            var news = new Command()
            {
                Title = "استخدام",
                Code = "job",
                CommandType = CommandType.Category,
            };
            var news = new Command()
            {
                Title = "پربیننده ترین مطالب",
                CommandType = CommandType.Category,
                Code = "top",
            };
            var moreTeam = new Command()
            {
                Title = "سایتهای خبری",
                Code = "sites",
                CommandType = CommandType.Custom,
                Commands = string.Format("[[\"{0}\",\"{1}\",\"{2}\"],[\"{5}\",\"{3}\",\"{4}\"],[\"{6}\",\"{7}\",\"{8}\"]]", "asriran.com", "tabnak.ir", "jamejamonline.ir", "varzesh3", "p30download.com", "zoomit.ir", "niksalehi.com", "hidoctor", "سایت‌های بیشتر")
            };
            var moreTeam = new Command()
            {
                Title = "سایت‌های بیشتر",
                Code = "moresites",
                CommandType = CommandType.Custom,
                Commands = string.Format("[[\"{0}\",\"{1}\",\"{2}\"],[\"{5}\",\"{3}\",\"{4}\"],[\"{6}\",\"{7}\",\"{8}\"]]", "aftabnews.ir", "entekhab.ir", "webgardi2030.ir", "esteghlali.com", "tarafdari.com", "digiato.com", "persianv.com", "topnaz.com", "منوی اصلی")
            };
        }
    }
}
