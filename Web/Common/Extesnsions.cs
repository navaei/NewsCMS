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


public static class MNExtesnsions
{
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
        if ((Length - StartIndex) < str.Length)
            return str.Substring(StartIndex, Length);
        else
            return str;
    }
    public static string SubstringM(this string str, int StartIndex, int Length)
    {
        if ((Length - StartIndex) < str.Length)
            return str.Substring(StartIndex, Length);
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
    public static void AddToChache_Hours(this System.Web.Caching.Cache cache, string Key, object Value, int Hour)
    {
        cache.Insert(Key, Value, null, DateTime.Now.AddHours(Hour), System.Web.Caching.Cache.NoSlidingExpiration);
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

    public static FileContentResult ImageRender(this HtmlHelper helper, byte[] ImagByte)
    {
        return new FileContentResult(ImagByte, "image/png");
    }


}


