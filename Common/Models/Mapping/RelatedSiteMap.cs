using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class RelatedSiteMap : EntityTypeConfiguration<RelatedSite>
    {
        public RelatedSiteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("RelatedSites");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MainSiteId).HasColumnName("MainSiteId");
            this.Property(t => t.RelatedSiteId).HasColumnName("RelatedSiteId");
        }
    }
}
