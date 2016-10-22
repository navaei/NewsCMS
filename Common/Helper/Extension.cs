using System;
using System.Collections.Generic;
using System.Linq;
using Mn.NewsCms.Common.BaseClass;

namespace Mn.NewsCms.Common.Helper
{
    public static class Extension
    {
        public static string ToPersian(this DateTime date, string format = "yyyy/MM/dd")
        {
            //return date.ToString(format,DateTimeHelper.GetPersianCulture());
            return DateTimeHelper.DateTimeToPersin(date);
        }
        public static DateTime ToPersian(this DateTime date)
        {
            return DateTimeHelper.DateToPersin(date);
        }
        public static DateTime ToEnglish(this DateTime date)
        {
            if (date.Year > 1900)
                return date;

            return DateTimeHelper.PersinToDateTime(date);
        }
        public static string ToPersianBeautiful(this DateTime date)
        {

            var res = "";
            if (date.Year < 1000)
                return res;

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            if (DateTime.Today.ToShortDateString() == date.ToShortDateString())
            {
                TimeSpan span = DateTime.Now.Subtract(date);
                if (span.Hours > 0)
                {
                    res = DateTimeHelper.FarsiNumber(span.Hours.ToString()) + " ساعت پیش ";
                }
                else if (span.Minutes > 3)
                {
                    res = DateTimeHelper.FarsiNumber(span.Minutes.ToString()) + " دقیقه پیش";
                }
                else
                {
                    res = "همین لحظه";
                }
            }
            else
            {
                res = DateTimeHelper.FarsiNumber(pc.GetDayOfMonth(date).ToString() + " " + pc.GetMonthString(date).ToString() + " " + pc.GetYear(date).ToString());
            }
            return res;
        }
        public static string ToPersianBeautiful(this DateTime? date)
        {
            if (date.HasValue)
            {
                return ToPersianBeautiful(date.Value);
            }
            return string.Empty;
        }
        public static string GetMonthString(this System.Globalization.PersianCalendar pc, DateTime datetime)
        {
            int mont = pc.GetMonth(datetime);
            string[] monts = { "فروردین", "ادیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
            return monts.GetValue(mont - 1).ToString();
        }
        public static void AddEntities(this ICollection<IBaseEntity> entities, ICollection<IBaseEntity> newEntities)
        {
            List<IBaseEntity> Entities = entities.ToList();
            foreach (var entity in newEntities)
            {
                if (!newEntities.Any(e => e.Id == entity.Id))
                {
                    Entities.Add(entity);
                }
            }
            entities = Entities;
        }
        public static void AddEntities<T>(this ICollection<T> entities, List<T> newEntities) where T : IBaseEntity
        {
            foreach (var entity in newEntities)
                if (!entities.Any(e => e.Id == entity.Id))
                    entities.Add(entity);
        }
        public static void AddEntities2<T>(this ICollection<T> entities, List<T> newEntities) where T : BaseEntity<int>
        {
            foreach (var entity in newEntities)
                if (!entities.Any(e => e.Id == entity.Id))
                    entities.Add(entity);
        }
    }
}
