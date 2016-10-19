using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class SponserMap : EntityTypeConfiguration<Sponser>
    {
        public SponserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SponserName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SponserLink)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.SponserImage)
                .HasMaxLength(300);

            this.Property(t => t.OfficalWebSite)
                .HasMaxLength(300);

            this.Property(t => t.KeyWords)
                .HasMaxLength(300);

            this.Property(t => t.Controller)
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("Sponsers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SponserName).HasColumnName("SponserName");
            this.Property(t => t.SponserLink).HasColumnName("SponserLink");
            this.Property(t => t.SponserImage).HasColumnName("SponserImage");
            this.Property(t => t.OfficalWebSite).HasColumnName("OfficalWebSite");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.ExpireDate).HasColumnName("ExpireDate");
            this.Property(t => t.KeyWords).HasColumnName("KeyWords");
            this.Property(t => t.Actived).HasColumnName("Actived");
            this.Property(t => t.Controller).HasColumnName("Controller");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
