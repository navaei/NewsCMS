using System;
using System.Linq;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public partial class WebPartsContainer
    {
        [Key]
        public int WebPartId { get; set; }
        public string ContainerCode { get; set; }
        public virtual WebPart WebPart { get; set; }
    }
}
