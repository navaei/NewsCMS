using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class AdsContainerMap : EntityTypeConfiguration<AdsContainer>
    {
        public AdsContainerMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AdsId, t.ContainerCode });

            // Properties
            this.Property(t => t.AdsId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ContainerCode)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AdsContainer");
            this.Property(t => t.AdsId).HasColumnName("AdsId");
            this.Property(t => t.ContainerCode).HasColumnName("ContainerCode");

            // Relationships
            this.HasRequired(t => t.Ad)
                .WithMany(t => t.AdsContainers)
                .HasForeignKey(d => d.AdsId);

        }
    }
}
