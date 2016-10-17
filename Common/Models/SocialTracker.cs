using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tazeyab.Common.Models
{
    public partial class SocialTracker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SocialTrackerID { get; set; }
        public long? ItemRef { get; set; }
        public Guid? FeedItemRef { get; set; }
        public int SocialNetworkRef { get; set; }
        public System.DateTime CreationDate { get; set; }
    }
}
