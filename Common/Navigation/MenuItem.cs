using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Mn.NewsCms.Common.Navigation
{
    public class MenuItem : BaseEntity
    {
        public MenuItem()
        {
            Items = new List<MenuItem>();
        }

        [MaxLength(64)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [MaxLength(256)]
        [Display(Name = "لینک منو")]
        public string Url { get; set; }
        [MaxLength(64)]
        [Display(Name = "کد آیکون")]
        public string Icon { get; set; }
        public long MenuId { get; set; }
        [ForeignKey("MenuId")]
        [ScriptIgnore]
        public Menu Menu { get; set; }
        [Display(Name = "شناسه والد ")]
        public long? ParentItemId { get; set; }
        [ForeignKey("ParentItemId")]
        [ScriptIgnore]
        public MenuItem ParentItem { get; set; }
        [ScriptIgnore]
        public ICollection<MenuItem> Items { get; set; }
    }
}
