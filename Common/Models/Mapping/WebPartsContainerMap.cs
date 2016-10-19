using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class WebPartsContainerMap : EntityTypeConfiguration<WebPartsContainer>
    {
        public WebPartsContainerMap()
        {
            this.ToTable("WebPartsContainer");
        }
    }
}
