using System;
using System.Configuration;
using Microsoft.AspNetCore.Http;

namespace Mn.NewsCms.WebCore.WebLogic
{
    public enum ThemeType
    {
        Dark = 1,
        Light = 2
    }

    public static class CmsConfig
    {
        public const string ThemeName = "Sekuzan";

        public static ThemeType GetThemeType(HttpContext httpContext) => SelectedTheme(httpContext) == "DarkKnight" ? ThemeType.Dark : ThemeType.Light;
            
        public static string SelectedTheme(HttpContext httpContext)
        {

            try
            {
                return httpContext.Request.Cookies["SelectedTheme"];
            }
            catch
            {
                httpContext.Response.Cookies.Append("SelectedTheme", "DarkKnight");
                return "DarkKnight";
            }
        }

        public const int Cache1Min = 60;
        public const int Cache5Min = 300;//default cache time per second
        public const int Cache10Min = 600;
        public const int Cache30Min = 1800;
        public const int Cache1Hour = 3600;
        public const int Cache3Hour = 10800;
        public const int CacheHalfDay = 43200;

        public static int ItemsCacheTime => 10;

        public static string SoundcloudApiKey => ConfigurationManager.AppSettings["SoundcloudApiKey"];
    }
}