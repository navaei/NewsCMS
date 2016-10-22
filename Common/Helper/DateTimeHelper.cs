using System;
using System.Globalization;
using System.Reflection;

namespace Mn.NewsCms.Common.Helper
{
    public static class DateTimeHelper
    {
        private static CultureInfo _Culture;
        public static CultureInfo GetPersianCulture()
        {
            if (_Culture == null)
            {
                _Culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند",
                    ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_Culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _Culture.NumberFormat.NumberDecimalSeparator = "/";
                _Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _Culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _Culture;
        }

        public static DateTime PersinToDateTime(DateTime persianDate)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.ToDateTime(persianDate.Year, persianDate.Month, persianDate.Day, 0, 0, 0, 0, 0);
        }
        public static DateTime PersinToDateTime(string persianDate)
        {
            string[] formats = { "yyyy/MM/dd", "yyyy/M/d", "yyyy/MM/d", "yyyy/M/dd" };
            DateTime d1 = DateTime.ParseExact(persianDate, formats,
                                              CultureInfo.CurrentCulture, DateTimeStyles.None);
            PersianCalendar persian_date = new PersianCalendar();
            DateTime dt = persian_date.ToDateTime(d1.Year, d1.Month, d1.Day, 0, 0, 0, 0, 0);
            return dt;
        }


        public static string DateTimeToPersin(DateTime? date)
        {
            if (!date.HasValue)
                return string.Empty;
            PersianCalendar pc = new PersianCalendar();
            var year = pc.GetYear(date.Value);
            var month = pc.GetMonth(date.Value);
            var day = pc.GetDayOfMonth(date.Value);
            return year + "/" + month + "/" + day;
        }
        public static DateTime DateToPersin(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            var year = pc.GetYear(date);
            var month = pc.GetMonth(date);
            var day = pc.GetDayOfMonth(date);
            var enDate = new DateTime(year, month, day);
            return enDate;
        }


        public static string FarsiNumber(string str)
        {
            string s = "";
            int i;
            char[] ch = str.ToCharArray();
            foreach (char c in ch)
            {
                if (char.IsDigit(c))
                {
                    i = (int)char.GetNumericValue(c) + 1776;
                    s += ((char)i).ToString();
                }
                else
                {
                    s += c.ToString();
                }
            }
            return s;
        }
    }
}
