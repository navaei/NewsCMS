using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common.Membership;
using Mn.NewsCms.Common.Navigation;

namespace Mn.NewsCms.Common
{
    public enum PostType : byte
    {
        News = 0,
        Page = 1,
        Tab = 2,
        Section = 3,//footer,copyright,description,....
        Video = 4,
    }
    public class Post : BaseEntity
    {
        public Post()
        {
            EnableComment = false;
            Categories = new List<Category>();
            Tags = new List<Tag>();
            VisitCount = 1;
        }

        [MaxLength(256)]
        [Required]
        public string Title { get; set; }

        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        [MaxLength(512)]
        public string SubTitle { get; set; }
        public DateTime? PublishDate { get; set; }
        [MaxLength(256)]
        public string PostImage { get; set; }
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        public bool EnableComment { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int VisitCount { get; set; }
        public bool ShowInIndex { get; set; }
        public PostType PostType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public MetaData MetaData { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
