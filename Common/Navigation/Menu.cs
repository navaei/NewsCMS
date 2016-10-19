using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.NewsCms.Common.Navigation
{
    public enum MenuLocation
    {
        Top = 0,
        RightSide = 1,
        LeftSide = 2,
        Footer = 3,
        TopUnderBanner = 4,        
    }
    public class Menu : BaseEntity
    {
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Title { get; set; }
        public MenuLocation Location { get; set; }
        public bool EnableCategory { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
