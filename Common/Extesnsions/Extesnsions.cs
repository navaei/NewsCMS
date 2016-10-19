using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using Rss;
using Mn.Framework.Common;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.Share;


namespace Mn.NewsCms.Common
{
    public static class PublicExtesnsions
    {
        //------------string----------------------
        #region string
        public static int CountStringOccurrences(this string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.ToUpper().IndexOf(pattern.ToUpper(), i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
        public static int WordCount(this string str)
        {
            return str.Length;
        }
        public static bool ContainsX(this string str, string str2)
        {
            return str.ToUpper().Contains(str2.ToUpper());
        }
        public static string ReplaceX(this string strsource, string str1, string str2)
        {
            return strsource.Replace(str1, str2).Replace(str1.ToUpper(), str2.ToUpper());
        }
        public static string ReplaceAnyCase(this string strsource, string str1, string str2)
        {
            return strsource.Replace(str1.ToLower(), str2.ToLower());
        }
        public static int IndexOfX(this string strSource, string str)
        {
            return strSource.ToUpper().IndexOf(str.ToUpper());
        }
        public static bool EqualsX(this string str, string str2)
        {
            return str.ToUpper().Equals(str2.ToUpper());
        }
        public static string SubstringX(this string str, int StartIndex, int Length)
        {
            if ((Length - StartIndex) < str.Length)
                return str.Substring(StartIndex, Length);
            else
                return str;
        }
        public static string ApplyUnifiedYeKe(this string data)
        {
            if (string.IsNullOrEmpty(data)) return data;
            return data.Replace("ي", "ی").Replace("ك", "ک");
        }
        #endregion
        //------------Date Time--------------
        #region DateTime
        public static int NowHour(this DateTime date)
        {
            return DateTime.UtcNow.AddHours(ServiceFactory.Get<IAppConfigBiz>().GetConfig<double>("UTCDelay")).Hour;
        }
        #endregion
        //---------------list<string>------------
        #region list
        public static bool AddGoodURL(this List<string> list, string str)
        {
            str = str.Replace("//", "/").ReplaceX("http:/", "http://");
            str = str.ToUpper();
            if (str.IndexOf("?") != str.LastIndexOf("?"))
            {
                int point = str.IndexOf("?", str.IndexOf("?") + 1);
                str = str.Remove(point);
            }
            if (!list.Contains(str))
            {
                list.Add(str);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool AddX(this List<string> list, string str)
        {
            str = str.Replace("//", "/").ReplaceX("http:/", "http://");
            str = str.ToUpper();
            if (!list.Contains(str))
            {
                list.Add(str);
                return true;
            }
            else
            {
                return false;
            }
        }
        static Random random = new Random();
        public static IEnumerable<T> RandomPermutation<T>(this IEnumerable<T> sequence)
        {
            T[] retArray = sequence.ToArray();
            for (int i = 0; i < retArray.Length - 1; i += 1)
            {
                int swapIndex = random.Next(i + 1, retArray.Length);
                T temp = retArray[i];
                retArray[i] = retArray[swapIndex];
                retArray[swapIndex] = temp;
            }

            return retArray;
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            return Shuffle(items, random);
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items, Random random)
        {
            // Un-optimized algorithm taken from
            // http://en.wikipedia.org/wiki/Knuth_shuffle#The_modern_algorithm
            List<T> list = new List<T>(items);
            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = random.Next(0, i);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        #endregion
        //------------------Dictionary----------------------
        #region Dictionary
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dict,
                                     Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            foreach (var cur in dict.Where(condition).ToList())
            {
                dict.Remove(cur.Key);
            }
        }
        #endregion
        //-----------------console--------------------------
        #region feeds
        public static IEnumerable<FeedContract> ConvertToFeedContract(this List<Feed> feeds)
        {
            return feeds.DistinctBy(f => f.Id).DistinctBy(f => f.Link).Select(x => new FeedContract
            {
                Id = x.Id,
                Cats = string.Join(" ", x.Categories.Select(c => c.Id)),
                IsAtom = x.FeedType.HasValue && x.FeedType.Value == FeedType.Atom ? true : false,
                LastFeedItemUrl = x.LastUpdatedItemUrl,
                Link = x.Link,
                SiteId = x.SiteId,
                SiteTitle = x.Site.SiteTitle,
                SiteUrl = x.Site.SiteUrl,
                ShowContentType = x.Site.ShowContentType
            });
        }
        public static RssItemCollection GetRssItemCollection(this SyndicationFeed atomfeed)
        {
            int i = 0;
            List<RssItem> retlist = new List<RssItem>();
            RssItemCollection colection = new RssItemCollection();
            if (atomfeed.Items.Any())
                foreach (SyndicationItem item in atomfeed.Items)
                {
                    i++;
                    break;
                }
            if (i > 0)
                retlist = atomfeed.Items.Select(x => new RssItem { Title = x.Title.Text, Description = x.Summary.Text, Link = x.Links[0].Uri, PubDate = x.PublishDate.DateTime }).ToList();

            foreach (var item in retlist.OrderBy(x => x.PubDate))
                colection.Add(item);
            return colection;
        }
        //public static T getPropertyValue<T>(this FeedContract feed, string PropertyName)
        //{
        //    if (!string.IsNullOrEmpty(feed.PropertyVal))
        //    {

        //        //var proper = proper.Split(":");
        //        T retVal = feed.GetType().GetProperty(PropertyName).GetValue(feed, null);
        //        return retVal;
        //        //dbfeed.GetType().GetProperty(proper[0]).SetValue(dbfeed, proper[1], null);                    
        //    }
        //    return null;
        //}
        //public static void setPropertyValue(this FeedContract feed, string PropertyName, string PropertyValue)
        //{
        //    if (!string.IsNullOrEmpty(feed.PropertyVal))
        //    {
        //        var arr = feed.PropertyVal.Split(";");
        //        if (arr.Length > 0)
        //        {
        //            foreach (var properval in arr)
        //            {
        //                var proper = proper.Split(":");
        //                T retVal = feed.GetType().GetProperty(PropertyName).SetValue(feed,PropertyValue null);
        //                return retVal;
        //                //dbfeed.GetType().GetProperty(proper[0]).SetValue(dbfeed, proper[1], null);
        //            }
        //        }
        //    }
        //}
        #endregion

        #region Entity
        public static List<T> FromHttpCache<T>(this DbSet<T> context) where T : class
        {
            var type = typeof(T);
            if (System.Web.HttpRuntime.Cache[type.Name] != null)
                return System.Web.HttpRuntime.Cache[type.Name] as List<T>;
            else
            {
                var ret = context.ToList();
                System.Web.HttpRuntime.Cache[type.Name] = ret;
                return ret;
            }

        }
        #endregion

        //------------------Image--------------------
        public static byte[] ToByteArray(this System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        public static Image ToImage(this byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            return Image.FromStream(ms);
        }
    }

}
