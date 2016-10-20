using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
