using System.Collections.Generic;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.WebCore.Models
{
    public class ToolBoxDemo
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public string PagePath { get; set; }
        public string IframeSrc { get; set; }
        public string Script { get; set; }
        public List<Category> Cats { get; set; }
        public List<Common.Tag> Tags { get; set; }
    }
}
