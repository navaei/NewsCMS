using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class ItemVisitedMap : EntityTypeConfiguration<ItemVisited>
    {
        public ItemVisitedMap()
        {
            // Primary Key
            this.HasKey(t => t.ItemVisitedId);

            // Properties
            this.Property(t => t.FeedItemId)
                .IsRequired().HasMaxLength(50);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.EntityRef)
              .IsRequired();

            this.Property(t => t.EntityCode)
               .IsRequired()
               .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ItemVisited");
            this.Property(x => x.ItemVisitedId).HasColumnName("ItemVisitedID");
            this.Property(t => t.FeedItemId).HasColumnName("FeedItemId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Link).HasColumnName("Link");
            this.Property(t => t.VisitCount).HasColumnName("VisitCount");
            this.Property(t => t.EntityCode).HasColumnName("EntityCode");
            this.Property(t => t.EntityRef).HasColumnName("EntityRef");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.PubDate).HasColumnName("PubDate");
        }
    }
}
