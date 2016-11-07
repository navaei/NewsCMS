namespace Mn.NewsCms.WebCore.Models.WebPart
{
    public class WpWidget
    {
        public WpWidget()
        {
            HeaderColor = "#9d9d9d";
        }
        public string Title { get; set; }
        public string Code { get; set; }
        public string CssClass { get; set; }
        public string HeaderColor { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
    }
}