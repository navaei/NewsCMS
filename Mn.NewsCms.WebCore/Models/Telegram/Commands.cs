namespace Mn.NewsCms.WebCore.Models.Telegram
{
    public enum CommandType
    {
        Custom,
        Category,
        Tag,        
        Site,
    }
    public class Command
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public CommandType CommandType { get; set; }
        public string Messages { get; set; }
        public string Commands { get; set; }
    }

}
