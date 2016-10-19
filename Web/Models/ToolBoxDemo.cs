using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.Common.ViewModels
{
    public class ToolBoxDemo
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public string PagePath { get; set; }
        public string IframeSrc { get; set; }
        public string Script { get; set; }
        public List<Category> Cats { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
