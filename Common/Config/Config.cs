using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using Tazeyab.Common.Models;


namespace Tazeyab.Common
{
    public class Config
    {
        #region Property
        static int _DefaultSearchDuration = 0;
        static int _MaxDesciptionLength = 1800;
        static Dictionary<string, string> ConfigDic = new Dictionary<string, string>();
        static NameValueCollection appSettings = ConfigurationManager.AppSettings;
        #endregion
        public static string GetConfig(string Name)
        {

            if (ConfigDic.ContainsKey(Name))
                return ConfigDic.SingleOrDefault(x => x.Key == Name).Value;
            else
            {
                string Value;
                using (var entiti = new TazehaContext())
                {
                    if (entiti.ProjectSetups.Any(x => x.Title == Name))
                        Value = entiti.ProjectSetups.First(x => x.Title == Name).Value;
                    else
                        Value = appSettings.Get(Name);

                    lock (ConfigDic)
                        ConfigDic.Add(Name, Value);
                }
                return Value;
            }
        }
        public static T GetConfig<T>(string Name)
        {
            return (T)System.Convert.ChangeType(GetConfig(Name), Type.GetTypeCode(typeof(T)));
        }
        public static void SetConfig(string Name, string Value)
        {
            TazehaContext entiti = new TazehaContext();
            var config = entiti.ProjectSetups.SingleOrDefault(x => x.Title == Name);
            if (config != null)
            {
                config.Value = Value;
            }
            else
                entiti.ProjectSetups.Add(new ProjectSetup { Title = Name, Value = Value });
            entiti.SaveChanges();
            //ConfigurationManager.AppSettings[Name.Trim()] = Value;
        }
        public static int DefaultSearchDuration
        {
            get
            {
                if (_DefaultSearchDuration == 0)
                {
                    TazehaContext entiti = new TazehaContext();
                    UpdateDuration def = entiti.UpdateDurations.Single<UpdateDuration>(x => x.IsDefault.Value == true);
                    _DefaultSearchDuration = def.Id;
                }
                return _DefaultSearchDuration;
            }
        }
        public static int MaxDescriptionLength
        {
            get
            {
                if (_MaxDesciptionLength == 0)
                {
                    TazehaContext entiti = new TazehaContext();
                    var res = entiti.ProjectSetups.SingleOrDefault(x => x.Title == "MaxDescriptionLength");
                    _MaxDesciptionLength = int.Parse(res.Value.Trim());
                }
                return _MaxDesciptionLength;
            }
        }
        public static string[] RestrictSites()
        {
            string[] arr = { "facebook.com", "twitter.com", "blogspot.com", "blogger.com", "wordpress.com", "wordpress.org", "www.blogfa.com", "www.mihanblog.com" };
            return arr;
        }
        public static string LuceneLocation
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Server.MapPath("App_Data\\LuceneIndex");
                }
                return Config.GetConfig("LuceneLocation");
            }
        }
        public static int TimeInterval
        {
            get
            {
                return
                    GetConfig<int>("TimeInterval");
            }
        }//per minute
        public static void StartUpDailyConfiguration()
        {
            TazehaContext entiti = new TazehaContext();
        }
        public static int defCacheTimePerMin { get { return 10; } }
        public static bool ChkAppPrvcy()
        {
            string keyName = @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\services\pcmcia";
            string valueName = "AppIp";
            var val = Registry.GetValue(keyName, valueName, "NotExist").ToString();
            if (val == "NotExist")
            {
                return false;
            }
            else
            {
                if (HttpContext.Current == null || HttpContext.Current.Handler == null || HttpContext.Current.Request.IsLocal)
                    return true;
                else
                    return val == HttpContext.Current.Request.UserHostAddress && val == "95.211.12.38";
            }
        }
    }
}
