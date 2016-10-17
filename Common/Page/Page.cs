using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Tazeyab.Common
{
    public class Page : BaseEntity
    {
        public Page()
        {
            this.Categories = new List<Category>();
            this.Tags = new List<Tag>();
        }
        [Column("PageId")]
        public override int Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }
        public string PageCode { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Active { get; set; }
        public string Keywords { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        //public virtual ICollection<RemoteWebPart> RemoteWebParts { get; set; }

    }
}
