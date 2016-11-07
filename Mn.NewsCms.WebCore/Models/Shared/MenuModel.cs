using System.Collections.Generic;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Navigation;

namespace Mn.NewsCms.WebCore.Models.Shared
{
    public class MenuModel
    {
        public List<Category> Categories { get; set; }
        public Menu Menu { get; set; }
    }
}