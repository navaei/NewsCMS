using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Tazeyab.Common.Models.Mapping
{
    public class RemoteWebPartMap : EntityTypeConfiguration<RemoteWebPart>
    {
        public RemoteWebPartMap()
        {
            this.ToTable("RemoteWebParts");
            this.Property(t => t.WebPartCode)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.WebPartTitle)
                .IsRequired()
                .HasMaxLength(150);
            this.Property(t => t.Url)
                .IsRequired()
                .HasMaxLength(400);
            this.Property(t => t.Pattern)
                .IsRequired()
                .HasMaxLength(300);

            this.HasOptional(r => r.ParentRemoteWebPart).WithMany(r => r.RemoteWebParts).HasForeignKey(m => m.RemoteWebPartRef);
            //this.HasMany(r => r.RemoteWebParts).WithOptional(c => c.ParentRemoteWebPart).HasForeignKey(m => m.RemoteWebPartRef);
            this.HasMany(r => r.Tags).WithMany(t => t.RemoteWebParts);
            //this.HasMany(r => r.Categories).WithMany(c => c.RemoteWebParts);
        }
    }
}
