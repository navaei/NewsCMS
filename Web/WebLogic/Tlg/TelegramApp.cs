using Microsoft.Practices.Unity;
using Mn.Framework.Common;
using Mn.Framework.Common.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Config;
using Tazeyab.Common.EventsLog;
using Tazeyab.Common.ExternalService;
using Tazeyab.Common.Models;
using Tazeyab.Common.Share;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Tazeyab.Web.Tlg
{
    public static class TelegramApp
    {
        #region Properties
        public static bool StopApp = false;
        public static bool Busy = false;
        public static long TelegramLastUpdateId = 0;
        public static string LastString = string.Empty;
        public static List<TelegramMessage> Messages { get; set; }
        public static List<Command> Commands { get; set; }
        public static Dictionary<string, string> Photos { get; set; }
        static ICategoryBusiness CatBiz { get; set; }
        static ISiteBusiness SiteBiz { get; set; }
        static ITagBusiness TagBiz { get; set; }
        static IFeedItemBusiness ItemBiz { get; set; }
        static IAppConfigBiz appConfig { get; set; }
        static string baseUrl { get; set; }
        static string siteName { get; set; }
        static string token { get; set; }
        static int pageSize { get; set; }
        static Api Bot { get; set; }

        #endregion

        public static bool ProcessMessage(Message message)
        {
            if (string.IsNullOrEmpty(message.text) && message.Photo == null && message.Document == null)
                return false;

            GeneralLogs.WriteLog("tlg: " + message.chat.id + " " + message.from.first_name + " " + message.from.username + " " + message.text, TypeOfLog.Info, "Telegram");
            if (message.Document != null)
                GeneralLogs.WriteLog("tlg: document " + message.Document.file_name + " " + message.Document.file_id, TypeOfLog.Info, "Telegram");
            if (message.Photo != null)
            {
                GeneralLogs.WriteLog("tlg: photo " + message.Photo.First().file_id, TypeOfLog.Info, "Telegram");
                return false;
            }

            if (Messages.Count > 20)
                InsertMessagesToDb();

            var newsUrl = baseUrl;
            var contentType = ContentType.Cat;
            var res = new List<FeedItem>();
            string title = string.Empty;

            var comand = Commands.SingleOrDefault(c => c.Title.StartsWith(message.text.Replace("/", "")) || message.text.EndsWith(c.Title) || c.Code == message.text.ToLower().Replace("/", "").Trim());
            if (comand != null)
            {
                if (comand.CommandType == CommandType.Custom)
                {
                    if (comand.Code == "top")
                    {
                        res = ItemBiz.MostVisitedItems("today", 10, pageSize, 30).Select(i => new FeedItem()
                        {
                            Id = i.Id,
                            Title = i.Title,
                            SiteTitle = i.SiteTitle,
                            SiteUrl = i.SiteUrl,
                            Description = i.Description
                        }).ToList();
                        newsUrl += "items/todaymostvisited";
                    }
                    else if (comand.Code == "contact")
                    {
                        var contactMsg = "جهت تماس با ما میتوانید به شناسه @MNavaei پیام دهید \n و یا به نشانی Info@Tazeyab.com ایمیل ارسال فرمایید";
                        SendMessage(message.chat.id, contactMsg, comand.Keybord);
                        return true;
                    }
                    else
                    {
                        SendMessage(message.chat.id, comand.Messages, comand.Keybord);
                        return true;
                    }
                }
                else if (comand.CommandType == CommandType.Category)
                {
                    res = ItemBiz.FeedItemsByCat(comand.Code, pageSize, 0, true, 20);
                    title = comand.Code;
                    newsUrl += contentType.ToString().ToLower() + "/" + title.Replace(" ", "_");

                }
                else
                {
                    contentType = ContentType.Tag;
                    title = comand.Title;
                    res = ItemBiz.FeedItemsByTag(comand.Title, pageSize, 0, ref title, 20);
                    newsUrl += contentType.ToString().ToLower() + "/" + title.Replace(" ", "_");
                }
            }
            else
            {
                if (message.text.Contains(".") && message.text.IndexOf(".") == message.text.LastIndexOf("."))
                {
                    var site = SiteBiz.Get(message.text.Trim());
                    if (site != null)
                    {
                        contentType = ContentType.Site;
                        title = message.text;
                        res = ItemBiz.FeedItemsBySite(site.Id, pageSize, 0, 20);
                        newsUrl += contentType.ToString().ToLower() + "/" + title.Replace(" ", "_");
                        comand = new Command() { Code = site.SiteUrl, Title = site.SiteTitle, CommandType = CommandType.Site };
                    }
                }
                else if (message.text.StartsWith("📄"))
                {
                    //------newspaper------
                    if (Photos.Any(p => p.Key == message.text.Replace("📄", "")))
                    {
                        SendPhoto(message.chat.id, Photos.First(p => p.Key == message.text.Replace("📄", "")).Value, null, null, Photos.First(p => p.Key == message.text.Replace("📄", "")).Value);
                        return true;
                    }
                    else
                    {
                        var today = DateTime.Now.AddHours(-DateTime.Now.Hour);
                        var yesterday = DateTime.Now.AddDays(-1);
                        var photo = Ioc.DataContext.PhotoItems.SingleOrDefault(x => x.Title.Replace(" ", "").StartsWith(message.text.Replace("📄", "").Replace(" ", "")) &&
                          message.text.Replace("📄", "").Replace(" ", "").StartsWith(x.Title.Replace(" ", "")) && x.CreationDate > today);

                        if (photo == null)
                            photo = Ioc.DataContext.PhotoItems.SingleOrDefault(x => x.Title.Replace(" ", "").StartsWith(message.text.Replace("📄", "").Replace(" ", "")) &&
                          message.text.Replace("📄", "").Replace(" ", "").StartsWith(x.Title.Replace(" ", "")) && x.CreationDate > yesterday); ;

                        if (photo != null)
                        {
                            var file = new FileStream(HttpContext.Current.Server.MapPath("~/PhotoFiles/NewsPaper/" + photo.PhotoURL), FileMode.Open);
                            var sentRes = SendPhoto(message.chat.id, photo.Id.ToString(), file, null, string.Empty);
                            Photos.Add(message.text.Replace("📄", ""), sentRes.Photo.First().FileId);
                            return true;
                        }
                    }
                    return false;
                }
                else if (!string.IsNullOrEmpty(message.text) && message.text.Trim().Length > 1 && message.text.Length < 50)
                {
                    var tag = TagBiz.Get(message.text.Trim());
                    if (tag != null)
                    {
                        contentType = ContentType.Tag;
                        title = message.text;
                        res = ItemBiz.FeedItemsByTag(tag, pageSize, 0, 20);
                        newsUrl += contentType.ToString().ToLower() + "/" + title.Replace(" ", "_");
                        comand = new Command() { Title = tag.Title, Code = tag.EnValue, CommandType = CommandType.Tag };
                    }
                    else
                    {
                        contentType = ContentType.Key;
                        title = message.text;
                        res = ItemBiz.FeedItemsByKey(message.text, pageSize, 0, 20);
                        newsUrl += contentType.ToString().ToLower() + "/" + title.Replace(" ", "_");
                        comand = new Command() { Title = title, Code = title, CommandType = CommandType.Key };

                    }
                }
                else
                {
                    //SendMessage(message.chat.id, "دستور یافت نشد", "");
                }
            }

            if (res.Any())
            {
                //var count = 1;
                //var msg = string.Join("\n\n", res.Select(i => (count++) + "\u20e3 " +
                //    i.SiteTitle.SubstringM(0, 25) +
                //    ":" + i.Title.Replace("\"", " ") + "\n" +
                //    i.Description.Replace("\"", " ").SubstringETC(0, 300)));
                //msg += "\n\n متن کامل و خبرهای بیشتر \n" + newsUrl;
                //msg = siteName + "\n" + msg;               
                foreach (var item in res)
                {
                    var msg = item.Title + "\n";
                    if (comand == null || (comand.CommandType != CommandType.Category && comand.CommandType != CommandType.Site))
                        msg += item.Description.Replace("\"", " ").SubstringETC(0, 300) + " \n";
                    msg += " @TazeyabBot \n";
                    msg += baseUrl.Replace("www.", "") + "site/t/" + item.ItemId;
                    SendMessage(message.chat.id, msg, comand != null ? comand.Keybord : null,
                        comand != null && (comand.CommandType == CommandType.Category || comand.CommandType == CommandType.Site) ? false : true);
                }
                return true;
            }

            return false;
        }
        public static void SendMessage(int chat_id, string message, ReplyMarkup replyMarkup, bool disableWebPagePreview = true)
        {
            Bot.SendTextMessage(chat_id, message, disableWebPagePreview, 0, replyMarkup);
        }
        public static void SendMessage(int chat_id, string message, string keyboard, bool hideKeyboard = false)
        {
            WebRequest req = null;
            try
            {
                var url = "https://api.telegram.org/" + token + "/sendMessage?chat_id=" + chat_id;
                //if (!string.IsNullOrEmpty(message))
                url += "&text=" + message;
                if (!string.IsNullOrEmpty(keyboard))
                    url += string.Format("&reply_markup={{\"keyboard\":{0},\"one_time_keyboard\":true,\"resize_keyboard\":false,\"force_reply\":false,\"hide_keyboard\":{1} }}", keyboard, hideKeyboard.ToString().ToLower());

                req = WebRequest.Create(url);
                req.UseDefaultCredentials = true;
                var result = req.GetResponse();
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog(ex.Message, TypeOfLog.Error, "Telegram");
            }
            finally
            {
                req.Abort();
            }
        }
        public static Telegram.Bot.Types.Message SendPhoto(int chat_id, string fileName, Stream stream, ReplyMarkup replyMarkup, string photo)
        {
            if (!string.IsNullOrEmpty(photo))
            {
                return Bot.SendPhoto(chat_id, photo).Result;
            }
            else
            {
                fileName = fileName.Replace(" ", "_") + ".jpg";
                FileToSend photoFile = new FileToSend(fileName, stream);
                return Bot.SendPhoto(chat_id, photoFile).Result;
            }
        }

        public static void Init()
        {
            CatBiz = ServiceFactory.Get<ICategoryBusiness>();
            SiteBiz = ServiceFactory.Get<ISiteBusiness>();
            TagBiz = ServiceFactory.Get<ITagBusiness>();
            ItemBiz = ServiceFactory.Get<IFeedItemBusiness>();
            appConfig = ServiceFactory.Get<IAppConfigBiz>();
            baseUrl = ConfigurationManager.AppSettings["SiteUrl"] + "/";
            siteName = ConfigurationManager.AppSettings["SiteName"];
            token = ConfigurationManager.AppSettings["TelegramAppId"];
            Bot = new Telegram.Bot.Api(token.Replace("bot", ""));
            pageSize = int.Parse(ConfigurationManager.AppSettings["TelegramPageSize"]);
            Photos = new Dictionary<string, string>();

            initCommands();
            Messages = new List<TelegramMessage>();
            var telg = appConfig.GetConfig("TelegramLastUpdateId");
            if (!string.IsNullOrEmpty(telg))
                TelegramLastUpdateId = long.Parse(telg);
        }
        static void initCommands()
        {
            Commands = new List<Command>();
            var topKeys = ServiceFactory.Get<ITagBusiness>().GetListRecentKeyWord().ToList().Select(key => new string[] { key.Title }).ToList();
            var start = new Command()
            {
                Title = "منوی اصلی",
                Code = "start",
                Icon = "🌐",
                Messages = "از چی میخوای باخبر بشی؟",
                CommandType = CommandType.Custom,
                Keybord = new ReplyKeyboardMarkup()
                {
                    Keyboard = new string[][] {
                    new string[] { "🌐 اخبارایران‌وجهان","⚽ تازه‌های فوتبالی" },
                    new string[] { "💻 دنیای فناوری","👨 استخدام" },
                    new string[] { "🎶 سرگرمی","🌟 مهارت‌های زندگی" },
                    new string[] { "📡 سایتهای خبری" },
                    new string[] { "💜 پربازدیدترین","🔥 سوژه‌های خبری" },
                    new string[] { "📰 صفحه نخست روزنامه‌ها" },
                    new string[] { "تماس با ما" } }
                }
            };
            Commands.Add(start);

            var news = new Command()
            {
                Title = "اخبارایران‌وجهان",
                Icon = "🌏",
                Messages = "آخرین اخبار ایران و جهان",
                Code = "news",
                CommandType = CommandType.Category,
            };
            Commands.Add(news);

            var newspaper = new Command()
            {
                Title = "صفحه نخست روزنامه‌ها",
                Code = "newspaper",
                Icon = "📰",
                Messages = "صفحه اول روزنامه های و نشریات امروز ایران",
                CommandType = CommandType.Custom,
                Keybord = new ReplyKeyboardMarkup()
                {
                    Keyboard = new string[][] {
                        new string[] { "📄 ایران","📄 جام جم"  },
                        new string[] { "📄 همشهری","📄 شرق"  },
                        new string[] { "📄 آفتاب یزد","📄 اعتماد" },
                        new string[] { "📄 کیهان" , "📄 وطن امروز"},
                        new string[] { "📄 جوان","📄 مردم سالاری" },
                        new string[] { "📄 خبر ورزشی","📄 ایران ورزشی" },
                        new string[] { "📄 هفت صبح","📄 بانی فیلم"  },
                        new string[] { "📄 آسیا","📄 دنیای اقتصاد" },
                        new string[] { "منوی اصلی" }
                    }
                }
            };
            Commands.Add(newspaper);

            var football = new Command()
            {
                Title = "تازه‌های فوتبالی",
                Icon = "⚽️",
                Messages = "جدیدترین اخبار و حواشی فوتبال داخلی و لیگ های خارجی",
                Code = "football",
                CommandType = CommandType.Category,
                Keybord = new ReplyKeyboardMarkup()
                {
                    Keyboard = new string[][] {
                        new string[] { "پرسپولیس", "استقلال"},
                        new string[] { "تراکتور","سپاهان"},
                        new string[] { "رئال مادرید","بارسلونا"},
                        new string[] { "منچستر","چلسی"},
                        new string[] { "یوونتوس","بایرن"},
                        new string[] { "اینترمیلان","آث میلان"},
                        new string[] { "منوی اصلی" }
                    }
                }
            };
            Commands.Add(football);
            var topTags = new Command()
            {
                Title = "سوژه‌های خبری",
                Code = "topTags",
                Icon = ":bomb:",
                Messages = "عناوین برگزیده و داغ خبری",
                CommandType = CommandType.Custom,
            };
            topKeys.Add(new string[] { "منوی اصلی" });
            topTags.Keybord = new ReplyKeyboardMarkup() { Keyboard = topKeys.ToArray() };
            Commands.Add(topTags);

            var it = new Command()
            {
                Title = "دنیای فناوری",
                Code = "it",
                Icon = "📡",
                CommandType = CommandType.Category,
            };
            Commands.Add(it);

            var fun = new Command()
            {
                Title = "سرگرمی",
                Code = "fun",
                Icon = "🎼",
                Messages = "تازه ترین اخبار دنیای سینما،فیلم و موسیقی",
                CommandType = CommandType.Category,
                Keybord = new ReplyKeyboardMarkup()
                {
                    Keyboard = new string[][] {
                        new string[] { "سینما","بازیگران"},
                        new string[] { "موسیقی","فیلم"},
                        new string[] { "آهنگ جدید"},
                        new string[] { "دانلود بازی"},
                        new string[] { "دانلود فیلم"},
                        new string[] { "فال روز"},
                        new string[] { "تلویزیون","سریال"},
                        new string[] { "منوی اصلی"}
                    }
                }
            };
            Commands.Add(fun);

            var jobs = new Command()
            {
                Title = "استخدام",
                Code = "job",
                Icon = "👥",
                CommandType = CommandType.Category,
            };
            var sucess = new Command()
            {
                Title = "مهارت‌های زندگی",
                Code = "success",
                Icon = "🌟",
                CommandType = CommandType.Category,
            };
            Commands.Add(sucess);

            var top = new Command()
            {
                Title = "پربازدیدترین",
                Code = "top",
                Icon = "👍",
                CommandType = CommandType.Custom,
            };
            Commands.Add(top);

            var contact = new Command()
            {
                Title = "تماس با ما",
                Code = "contact",
                Icon = "✉",
                CommandType = CommandType.Custom,
            };
            Commands.Add(contact);

            var sites = new Command()
            {
                Title = "سایتهای خبری",
                Code = "sites",
                Icon = "📣",
                Messages = "با انتخاب هر سایت تازه ترین عناوین خبری آنرا همینجا بخوانید",
                CommandType = CommandType.Custom,
                Keybord = new ReplyKeyboardMarkup()
                {
                    Keyboard = new string[][] {
                        new string[] { "asriran.com","tabnak.ir"},
                        new string[] { "entekhab.ir","aftabnews.ir"},
                        new string[] { "yjc.ir", "webgardi2030.ir"},
                        new string[] { "jamejamonline.ir"},
                        new string[] { "digiato.com","zoomit.ir"},
                        new string[] { "esteghlali.com","tarafdari.com"},
                        new string[] { "varzesh3.ir"},
                        new string[] { "topnaz.com","hidoctor.ir"},
                        new string[] { "niksalehi.com","persianv.com" },
                        new string[] { "منوی اصلی" }
                    }
                },
            };
            Commands.Add(sites);

        }

        static void InsertMessagesToDb()
        {
            var query = @"INSERT INTO [TelegramMessages]([Command],[UpdateId],[ChatId],[Name])VALUES";

            foreach (var msg in Messages)
            {
                query += string.Format("('{0}',{1}, {2},'{3}'),", msg.Command, msg.UpdateId, msg.ChatId, msg.Name);
            }

            ServiceFactory.DataContext.Database.ExecuteSqlCommand(query.Remove(query.Length - 1));
            Messages.Clear();
        }
    }
}
