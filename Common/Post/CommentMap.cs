using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            // Relationships
            this.HasOptional(t => t.CommentRef).WithMany().HasForeignKey(d => d.CommentRefId);
        }
    }
}
