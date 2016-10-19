using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class Tags_TempMap : EntityTypeConfiguration<Tags_Temp>
    {
        public Tags_TempMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TagTempId, t.Value });

            // Properties
            this.Property(t => t.TagTempId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Tags_Temp");
            this.Property(t => t.TagTempId).HasColumnName("TagTempId");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Ranking).HasColumnName("Ranking");
        }
    }
}
