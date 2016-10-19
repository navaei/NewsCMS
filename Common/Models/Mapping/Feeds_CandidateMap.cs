using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class Feeds_CandidateMap : EntityTypeConfiguration<Feeds_Candidate>
    {
        public Feeds_CandidateMap()
        {
            // Primary Key
            this.HasKey(t => t.FeedCandidateId);

            // Properties
            this.Property(t => t.SiteURL)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.FeedLink)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.SiteTitle)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Feeds_Candidate");
            this.Property(t => t.FeedCandidateId).HasColumnName("FeedCandidateId");
            this.Property(t => t.SiteURL).HasColumnName("SiteURL");
            this.Property(t => t.FeedLink).HasColumnName("FeedLink");
            this.Property(t => t.SiteTitle).HasColumnName("SiteTitle");
            this.Property(t => t.Id).HasColumnName("Id");
            

        }
    }
}
