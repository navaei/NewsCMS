using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Tazeyab.Common
{
    public class RemoteWebPart : BaseEntity
    {
        public RemoteWebPart()
        {
            this.RemoteWebParts = new HashSet<RemoteWebPart>();
        }

        [Column("WebPartID")]
        public override long Id
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
        public string WebPartCode { get; set; }
        public string WebPartTitle { get; set; }
        public string WebPartDesc { get; set; }
        public string Pattern { get; set; }
        public string Url { get; set; }
        public string UrlPattern { get; set; }
        public string Replace { get; set; }
        public int CacheTime { get; set; }
        public string Css { get; set; }
        public string JavaScript { get; set; }
        public string Keywords { get; set; }
        public string CssLink { get; set; }
        public bool DisableLink { get; set; }
        public bool Active { get; set; }
        public int FailedRequestCount { get; set; }
        public long? RemoteWebPartRef { get; set; }

        [NotMapped]
        public string InnerHtml { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        [ForeignKey("RemoteWebPartRef")]
        public virtual RemoteWebPart ParentRemoteWebPart { get; set; }
        public virtual ICollection<RemoteWebPart> RemoteWebParts { get; set; }
    }
}
