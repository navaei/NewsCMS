using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.NewsCms.Common.Entities.SiteMap
{
    public interface ISitemapItem
    {
        string Url { get; }
        DateTime? LastModified { get; }
        ChangeFrequency? ChangeFrequency { get; }
        float? Priority { get; }
    }
}
