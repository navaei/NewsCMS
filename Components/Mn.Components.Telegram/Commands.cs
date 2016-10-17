using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Components.Telegram
{
    public enum CommandType
    {
        Custom,
        Category,
        Tag

    }
    public class Command
    {
        public string Title { get; set; }        
        public string Code { get; set; }
        public CommandType CommandType { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Commands { get; set; }
    }

}
