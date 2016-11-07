using System;
using System.Collections.Generic;

namespace Mn.NewsCms.WebCore.Models.Shared
{
    public class SliderModel
    {
        public SliderModel()
        {
            Items = new List<SliderItemModel>();
            Height = 200;
        }
        public int Height { get; set; }
        public List<SliderItemModel> Items { get; set; }
        public string Code { get; set; }
    }
    public class SliderItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
        public int VisitsCount { get; set; }
        public string ImageURL { get; set; }
        public string Refrence { get; set; }

    }
}