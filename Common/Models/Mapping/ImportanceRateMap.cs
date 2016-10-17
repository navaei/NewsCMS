using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class ImportanceRateMap : EntityTypeConfiguration<ImportanceRate>
    {
        public ImportanceRateMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Rate, t.RowNumber });

            // Properties
            this.Property(t => t.Rate)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RowNumber)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ImportanceRate");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.RowNumber).HasColumnName("RowNumber");
        }
    }
}
