using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Web.WebLogic.Tlg;

namespace Tazeyab.Web.Tlg
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
        public Document Document { get; set; }
        public PhotoSize[] Photo { get; set; }
        public int date { get; set; }
        public string text { get; set; }
    }
    //public class MessageType
    //{
    //    public MessageType(string method, string contentParameter)
    //    {
    //        Method = method;
    //        ContentParameter = contentParameter;
    //    }

    //    public static MessageType TextMessage
    //    {
    //        get
    //        {
    //            return new MessageType("sendMessage", "text");
    //        }
    //    }
    //    public static MessageType PhotoMessage
    //    {
    //        get
    //        {
    //            return new MessageType("sendPhoto", "photo");
    //        }
    //    }

    //    public string Method { get; set; }

    //    public string ContentParameter { get; set; }
    //}   
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

    public class Document
    {
        public string file_id { get; set; }
        public string file_name { get; set; }
        public string mime_type { get; set; }
        public int file_size { get; set; }
        public Tazeyab.Web.WebLogic.Tlg.PhotoSize thumb { get; set; }
    }

}
