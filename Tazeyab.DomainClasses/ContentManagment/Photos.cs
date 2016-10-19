using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class ThumbnailStruct
    {
        public string Value;
        public string Title;
        public string Img;
    }
    public class Photos
    {
        public string getThumbnailRandomInToday(string CatCode)
        {
            try
            {
                TazehaContext context = new TazehaContext();
                var cat = context.Categories.SingleOrDefault(x => x.Code == CatCode);
                var items = context.PhotoItems.Where(x => x.Id == cat.Id && x.CreationDate.Value.Day == DateTime.Now.Day && x.CreationDate.Value.Month == DateTime.Now.Month && x.CreationDate.Value.Year == DateTime.Now.Year);
                if (items.Count() == 0)
                    for (int i = 1; i < 15; i++)
                    {
                        DateTime Yeasertday = DateTime.Now.AddDays(-i);
                        items = context.PhotoItems.Where(x => x.Id == cat.Id && x.CreationDate.Value.Day == Yeasertday.Day && x.CreationDate.Value.Month == Yeasertday.Month && x.CreationDate.Value.Year == Yeasertday.Year);
                        if (items.Count() > 0)
                            break;
                    }

                var item = items.Shuffle().First();
                return item.PhotoThumbnail;
            }
            catch (Exception)
            {

                return string.Empty;
            }

        }
        public List<ThumbnailStruct> getThumbnailRandomTagsCat(int EachTop)
        {
            TazehaContext entiti = new TazehaContext();
            if (HttpContext.Current.Cache.Get("ThumbnailRandomTagsCat") != null)
                return (List<ThumbnailStruct>)HttpContext.Current.Cache.Get("ThumbnailRandomTagsCat");
            List<ThumbnailStruct> items = entiti.Tags.Where(x => x.ImageThumbnail != null).Select(x => new ThumbnailStruct() { Value = string.IsNullOrEmpty(x.EnValue) ? "Tag/" + x.Title : "Tag/" + x.EnValue, Img = x.ImageThumbnail, Title = x.Title }).Shuffle().Take(EachTop).ToList();
            List<ThumbnailStruct> items2 = entiti.Categories.Where(x => x.ImageThumbnail != null).Select(x => new ThumbnailStruct() { Value = "Cat/" + x.Code, Img = x.ImageThumbnail, Title = x.Title }).Shuffle().Take(EachTop).ToList();
            items.AddRange(items2);
            items = items.Shuffle().ToList();
            HttpContext.Current.Cache.Insert("ThumbnailRandomTagsCat", items, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
            return items;
        }
    }
}