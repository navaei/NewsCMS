using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tazeyab.Web.Areas.Dashboard.Models.Share
{
    public enum NotificationType
    {
        Info,
        Alarm,
        Error,
        Success
    }
    public class Notification
    {
        public string Title { get; set; }
        public NotificationType Type { get; set; }
        public string CssClass
        {
            get
            {
                switch (Type)
                {
                    case NotificationType.Info:
                        return "fa-arrow-circle-right text-aqua";
                    case NotificationType.Alarm:
                        return "fa-warning text-yellow";
                    case NotificationType.Error:
                        return "fa-square text-red";
                    case NotificationType.Success:
                        return "fa-inbox text-green";
                    default:
                        break;
                }

                return string.Empty;
            }
        }
    }
    public class HeaderMessage
    {
        public long Id { get; set; }
        public string Date { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
    }
    public class Task
    {
        public string Title { get; set; }
        public NotificationType Type { get; set; }
        public int Percent { get; set; }
        public string CssClass
        {
            get
            {
                switch (Type)
                {
                    case NotificationType.Info:
                        return "progress-bar-aqua";
                    case NotificationType.Alarm:
                        return "progress-bar-yellow";
                    case NotificationType.Error:
                        return "progress-bar-red";
                    case NotificationType.Success:
                        return "progress-bar-green";
                    default:
                        break;
                }

                return string.Empty;
            }
        }
    }
    public class Header
    {
        public List<HeaderMessage> Messsages { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<Task> Tasks { get; set; }
        public Header()
        {
            Seed();
        }
        public void Seed()
        {
            Messsages = new List<HeaderMessage>() { new HeaderMessage() { 
                Date="دو ساعت پیش",
                Content="لطفا پیام مرا بخوانید",
                Sender="محمد طاهری",
            }, 
            new HeaderMessage() { 
                Date="سه ساعت پیش",
                Content="افزودن مورد نظر را انجام دهید",
                Sender="سارا تست",
            },
            new HeaderMessage() { 
                Date="دیروز",
                Content="یبرای تست مورد نظر پیام ارسال شد",
                Sender="ایمان مصفا",
            },
            };

            Notifications = new List<Notification>() { new Notification(){
            Title="5 کاربر جدید ثبت نام کردند",
            Type=NotificationType.Success,
            },
            new Notification(){
            Title="8 فید بدلیل مشکلات متوقف شدند",
            Type=NotificationType.Error,
            },
            new Notification(){
            Title="بروزرسانی بازه های فیدها را انجام دهد",
            Type=NotificationType.Alarm,
            },
            new Notification(){
            Title="از سیستم پشتیبان تهیه شد",
            Type=NotificationType.Alarm,
            }
            };

            Tasks = new List<Task>() { new Task(){
            Title="طراحی بخش مطالعات",
            Percent=20,
            Type=NotificationType.Success,
            },
            new Task(){
            Title=" تماس با آقای محمدی",
            Type=NotificationType.Error,
            Percent=40,
            },
            new Task(){
            Title=" برخی از کارهای مشتریان",
            Type=NotificationType.Alarm,
            Percent=60
            },
            new Task(){
            Title="  ساختن محیطی دلنشین تر برای کار",
            Type=NotificationType.Alarm,
            Percent=80
            }
            };
        }
    }
}