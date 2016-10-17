using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Tazeyab.Web.Tlg
{
    public enum CommandType
    {
        Custom,
        Category,
        Tag, 
        Key,       
        Site,        
    }
    public class Command
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public CommandType CommandType { get; set; }
        public string Messages { get; set; }
        //public string Commands { get; set; }
        public ReplyKeyboardMarkup Keybord { get; set; }
    }

}
