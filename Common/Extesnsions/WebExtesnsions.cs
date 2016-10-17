using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Script.Serialization;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web.Caching;


public static class WebExtesnsions
{
    private static readonly Regex MatchArabicHebrew =
                 new Regex(@"[\u0600-\u06FF,\u0590-\u05FF]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static string CorrectRtl(this string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return string.Empty;

        const char rleChar = (char)0x202B;
        if (MatchArabicHebrew.IsMatch(title))
            return rleChar + title;
        return title;
    }

    public static string CorrectRtlBody(this string body)
    {
        if (string.IsNullOrWhiteSpace(body)) return string.Empty;

        if (MatchArabicHebrew.IsMatch(body))
            return "<div style='text-align: right; font-family:tahoma; font-size:9pt;' dir='rtl'>" + body + "</div>";
        return "<div style='text-align: left; font-family:tahoma; font-size:9pt;' dir='ltr'>" + body + "</div>";
    }
    public static string SHA1(this string data)
    {
        using (var sha1 = new SHA1Managed())
        {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
        }
    }
    //------------string----------------------
    #region string
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
        if (string.IsNullOrEmpty(str))
            return str;
        if ((Length - StartIndex) < str.Length)
            return str.Substring(StartIndex, Length);
        else
            return str;
    }
    public static string SubstringM(this string str, int StartIndex, int Length)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        if ((Length - StartIndex) < str.Length)
            return str.Substring(StartIndex, Length);
        else
            return str;
    }
    public static string SubstringETC(this string str, int StartIndex, int Length)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        if ((Length - StartIndex) < str.Length)
            return str.Substring(StartIndex, Length) + "...";
        else
            return str;
    }
    public static int CountStringOccurrences(this string text, string pattern)
    {
        // Loop through all instances of the string 'text'.
        int count = 0;
        int i = 0;
        while ((i = text.IndexOf(pattern, i)) != -1)
        {
            i += pattern.Length;
            count++;
        }
        return count;
    }

    public static string RemoveBadCharacterInURL(this string url)
    {
        return url.Replace(" ", "-").
            Replace(".", "-").
            Replace("+", "-").
            Replace("*", "-").
            Replace("&", "-").
            Replace(":", "-").
            Replace("�", "-").
            Replace("%", "-").
            Replace('(', '-').
            Replace(')', '-').
            Replace('/', '-').
            Replace("'", "-").
            Replace('"', '-').
            Replace('!', '-').
            Replace('$', '-').
            Replace('#', '-').
            Replace('~', '-').
            Replace('`', '-').
            Replace('?', '-').
            Replace('؟', '-');
    }
    #endregion
    //---------------list<string>------------
    #region list
    public static bool AddGoodURL(this List<string> list, string str)
    {
        str = str.Replace("//", "/").Replace("http:/", "http://");
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
        str = str.Replace("//", "/").Replace("http:/", "http://");
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

    #endregion
    //-----------------Json--------------------------
    #region Json
    public static string ToJson<T>(/* this */ T value, Encoding encoding)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));

        using (var stream = new MemoryStream())
        {
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, encoding))
            {
                serializer.WriteObject(writer, value);
            }

            return encoding.GetString(stream.ToArray());
        }
    }
    public static string ToJSON(this object obj)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(obj);
    }
    public static string ToJSON(this object obj, int recursionDepth)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.RecursionLimit = recursionDepth;
        return serializer.Serialize(obj);
    }
    #endregion
    #region EF
    public static IEnumerable<TElement> SqlQueryCache_FirstParam<TElement>(this Database database, int MinutesCacheTime, string sql, params object[] parameters)
    {
        if (HttpContext.Current.Cache.Get(sql + parameters[0]) == null)
        {
            var res = database.SqlQuery<TElement>(sql, parameters).ToList();
            HttpContext.Current.Cache.Insert(sql + parameters[0], res, null, DateTime.Now.AddMinutes(MinutesCacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        return ((IEnumerable<TElement>)HttpContext.Current.Cache.Get(sql + parameters[0]));
    }
    public static IEnumerable<TElement> SqlQueryCache_SecondParam<TElement>(this Database database, int MinutesCacheTime, string sql, params object[] parameters)
    {
        if (HttpContext.Current.Cache.Get(sql + parameters[1]) == null)
        {
            var res = database.SqlQuery<TElement>(sql, parameters).ToList();
            HttpContext.Current.Cache.Insert(sql + parameters[1], res, null, DateTime.Now.AddMinutes(MinutesCacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        return ((IEnumerable<TElement>)HttpContext.Current.Cache.Get(sql + parameters[1]));
    }
    public static IEnumerable<TElement> SqlQueryCache_FirstSecondParam<TElement>(this Database database, int MinutesCacheTime, string sql, params object[] parameters)
    {
        if (HttpContext.Current.Cache.Get(sql + parameters[0] + parameters[1]) == null)
        {
            var res = database.SqlQuery<TElement>(sql, parameters).ToList();
            HttpContext.Current.Cache.Insert(sql + parameters[1], res, null, DateTime.Now.AddMinutes(MinutesCacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        return ((IEnumerable<TElement>)HttpContext.Current.Cache.Get(sql + parameters[0] + parameters[1]));
    }
    public static IEnumerable<TElement> SqlQueryCache_123Params<TElement>(this Database database, int MinutesCacheTime, string sql, params object[] parameters)
    {
        if (HttpContext.Current.Cache.Get(sql + parameters[0] + parameters[1] + parameters[2]) == null)
        {
            var res = database.SqlQuery<TElement>(sql, parameters).ToList();
            HttpContext.Current.Cache.Insert(sql + parameters[0] + parameters[1] + parameters[2], res, null, DateTime.Now.AddMinutes(MinutesCacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        return ((IEnumerable<TElement>)HttpContext.Current.Cache.Get(sql + parameters[0] + parameters[1] + parameters[2]));
    }

    #endregion
    #region Cache
    public static bool Exist(this System.Web.Caching.Cache cache, string Key)
    {
        if (cache[Key] == null)
            return false;
        else
            return true;
    }
    public static void AddToChache_Hours(this System.Web.Caching.Cache cache, string Key, object Value, int Hour)
    {
        cache.Insert(Key, Value, null, DateTime.Now.AddHours(Hour), System.Web.Caching.Cache.NoSlidingExpiration);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    /// <param name="Minutes">add cache time per minute</param>
    public static void AddToCache(this System.Web.Caching.Cache cache, string Key, object Value, int Minutes)
    {
        cache.Insert(Key, Value, null, DateTime.Now.AddMinutes(Minutes), System.Web.Caching.Cache.NoSlidingExpiration);
    }
    public static void Set_Add(this System.Web.Caching.Cache cache, string Key, object Value, int Hour)
    {
        if (cache[Key] == null)
            cache.Insert(Key, Value, null, DateTime.Now.AddHours(Hour), System.Web.Caching.Cache.NoSlidingExpiration);
        else
        {
            //object temp = cache[Key];
            cache.Remove(Key);
            cache.Insert(Key, Value, null, DateTime.Now.AddHours(Hour), System.Web.Caching.Cache.NoSlidingExpiration);
        }
    }
    public static void CacheInsert(this HttpContextBase httpContext, string key, object data, int durationMinutes)
    {
        if (data == null) return;
        httpContext.Cache.Add(
            key,
            data,
            null,
            DateTime.Now.AddMinutes(durationMinutes),
            TimeSpan.Zero,
            CacheItemPriority.AboveNormal,
            null);
    }

    public static T CacheRead<T>(this HttpContextBase httpContext, string key)
    {
        var data = httpContext.Cache[key];
        if (data != null)
            return (T)data;
        return default(T);
    }

    public static void InvalidateCache(this HttpContextBase httpContext, string key)
    {
        httpContext.Cache.Remove(key);
    }
    #endregion
    #region DateTime
    public static string GetMonthString(this System.Globalization.PersianCalendar pc, DateTime datetime)
    {
        int mont = pc.GetMonth(datetime);
        string[] monts = { "فروردین", "ادیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        return monts.GetValue(mont - 1).ToString();
    }
    #endregion

    //public static FileContentResult ImageRender(this HtmlHelper helper, byte[] ImagByte)
    //{
    //    return new FileContentResult(ImagByte, "image/png");
    //}

    //public static string CaptchaImage(this HtmlHelper helper)
    //{
    //    StringBuilder stringBuilder = new StringBuilder();
    //    stringBuilder.Append("<img src=\"../../AntiBotImage.ashx\"");
    //    stringBuilder.Append(" alt=\"CAPTCHA\" />");

    //    return stringBuilder.ToString();
    //}

    public static bool IsMobile(this HttpRequest Request)
    {
        if (Request.Browser.IsMobileDevice ||
            Request.Browser.Platform.Contains("android") ||
            Request.UserAgent.ContainsX("android") ||
            Request.UserAgent.ContainsX("iphone") ||
            Request.Browser.MobileDeviceModel == "IPhone")
            return true;
        else
            return false;
    }
    public static bool IsMobile(this HttpRequestBase Request)
    {
        if (Request.Browser.IsMobileDevice ||
            Request.Browser.Platform.Contains("android") ||
            Request.UserAgent.ContainsX("android") ||
            Request.UserAgent.ContainsX("iphone") ||
            Request.Browser.MobileDeviceModel == "IPhone")
            return true;
        else
            return false;
    }
}


