using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public partial class WebPart
    {
        public WebPart()
        {
            this.WebPartsContainers = new List<WebPartsContainer>();
        }

        public int WebPartId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string HtmlContent { get; set; }
        public Nullable<bool> Global { get; set; }
        public virtual ICollection<WebPartsContainer> WebPartsContainers { get; set; }
    }
}
