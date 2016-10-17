using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Comments1 = new List<Comment>();
        }

        public decimal CommentId { get; set; }
        public string SenderName { get; set; }
        public string EMail { get; set; }
        public decimal FeedItemId { get; set; }
        public Nullable<decimal> ParentId { get; set; }
        public string Value { get; set; }
        public Nullable<int> Vote { get; set; }
        public virtual ICollection<Comment> Comments1 { get; set; }
        public virtual Comment Comment1 { get; set; }
    }
}
