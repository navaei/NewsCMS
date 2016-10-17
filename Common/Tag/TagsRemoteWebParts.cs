using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public partial class TagsRemoteWebParts
    {
        public int TagRemoteWebPartId { get; set; }
        public int TagId { get; set; }
        public int RemoteWebPartId { get; set; }       
        public virtual Tag Tag { get; set; }
        public virtual RemoteWebPart RemoteWebPart { get; set; }
    }
}
