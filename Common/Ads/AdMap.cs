using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class AdMap : EntityTypeConfiguration<Ad>
    {
        public AdMap()
        {
            // Primary Key
            this.HasKey(a => a.Id).Property(a => a.Id).HasColumnName("AdsId");

            this.HasMany(a => a.Tags).WithMany(t => t.Ads);
            this.HasMany(a => a.Categories).WithMany(t => t.Ads);
        }
    }
}
