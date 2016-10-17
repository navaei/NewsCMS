using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class FeedLogMap : EntityTypeConfiguration<FeedLog>
    {
        public FeedLogMap()
        {
            this.ToTable("FeedLogs");

            this.HasRequired(t => t.Feed).WithMany().HasForeignKey(d => d.FeedId);

        }
    }
}
