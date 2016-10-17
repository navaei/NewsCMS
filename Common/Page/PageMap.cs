using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Tazeyab.Common.Models.Mapping
{
    public class PageMap : EntityTypeConfiguration<Page>
    {
        public PageMap()
        {
            this.ToTable("Pages");

            this.Property(t => t.PageCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);
            this.Property(t => t.Body)
                .IsRequired()
                .HasMaxLength(1500);

            this.HasMany(p => p.Categories).WithMany(c => c.Pages);//.Map(m => m.MapLeftKey("PageId").MapRightKey("CatId"));
            this.HasMany(p => p.Tags).WithMany(c => c.Pages);//.Map(m => m.MapLeftKey("PageId").MapRightKey("TagId"));

        }
    }
}
