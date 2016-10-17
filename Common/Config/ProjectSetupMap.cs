using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class ProjectSetupMap : EntityTypeConfiguration<ProjectSetup>
    {
        public ProjectSetupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Title)
            //    .IsRequired()
            //    .HasMaxLength(50);

            //this.Property(t => t.Value)
            //    .IsRequired()
            //    .HasMaxLength(200);

            //this.Property(t => t.Meaning)
            //    .IsRequired()
            //    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProjectSetup");
            //this.Property(t => t.Title).HasColumnName("Title");
            //this.Property(t => t.Value).HasColumnName("Value");
            //this.Property(t => t.Meaning).HasColumnName("Meaning");
        }
    }
}
