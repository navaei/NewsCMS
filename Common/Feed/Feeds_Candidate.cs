using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class Feeds_Candidate
    {
        public int FeedCandidateId { get; set; }
        public string SiteURL { get; set; }
        public string FeedLink { get; set; }
        public string SiteTitle { get; set; }
        public int Id { get; set; }
        public virtual Category Category { get; set; }
    }
}
