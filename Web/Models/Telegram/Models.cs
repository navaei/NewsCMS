using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Web.Models.Telegram
{
    public class TelegramData
    {
        public Result[] result;
    }
    public class Result
    {
        public long update_id { get; set; }
        public Message message { get; set; }
    }
    public class Message
    {
        public int message_id { get; set; }
        public Message_From from { get; set; }
        public Message_Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
    }
    public class Message_From
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
    }
    public class Message_Chat
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
    }
}
