using System.Collections.Generic;
using System.Linq;
using Mn.NewsCms.WebCore.Models.Tag;

namespace Mn.NewsCms.WebCore.Models
{
    public static class ModelToViewModelExtension
    {
        public static IEnumerable<TagViewModel> ToTagModel(this IEnumerable<Common.Tag> tags)
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
