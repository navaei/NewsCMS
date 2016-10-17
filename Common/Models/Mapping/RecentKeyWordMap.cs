using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class RecentKeyWordMap : EntityTypeConfiguration<RecentKeyWord>
    {
        public RecentKeyWordMap()
        {

            // Properties
            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .HasMaxLength(100);          

        }
    }
}
