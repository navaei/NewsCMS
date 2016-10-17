using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common.Models;
using Tazeyab.Web.Models;

namespace Tazeyab.Common.Models
{
    public static class ModelToViewModelExtension
    {
        public static IEnumerable<TagViewModel> ToTagModel(this IEnumerable<Tag> tags)
        {
            return tags.Select(t => new TagViewModel()
            {
                Title = t.Title,
                EnValue = t.EnValue,
                Color = t.Color,
                FontSize = 16,
                Thumbnail = t.ImageThumbnail
            });
        }
    }
}
