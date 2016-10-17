using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class AdsContainer
    {
        public int AdsId { get; set; }
        public string ContainerCode { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
