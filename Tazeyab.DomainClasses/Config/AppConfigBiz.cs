using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.Config
{
    public class AppConfigBiz : BaseBusiness<ProjectSetup>, IAppConfigBiz
    {
        private readonly IUnitOfWork _dbContext;

        #region Property
        static long _DefaultSearchDuration = 0;
        static int _MaxDesciptionLength = 1800;
        static Dictionary<string, string> ConfigDic = new Dictionary<string, string>();
        static NameValueCollection appSettings = ConfigurationManager.AppSettings;
        #endregion

        public AppConfigBiz(IUnitOfWork dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ProjectSetup> GetList()
        {
            return base.GetList();
        }
        public string GetConfig(string Name)
        {
            try
            {
                if (ConfigDic.ContainsKey(Name))
                    return ConfigDic.SingleOrDefault(x => x.Key == Name).Value;
                else
                {
                    string Value;
                    if (base.GetList().Any(x => x.Title == Name))
                        Value = base.GetList().First(x => x.Title == Name).Value;
                    else
                        Value = appSettings.Get(Name);

                    lock (ConfigDic)
                        ConfigDic.Add(Name, Value);

                    return Value;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public T GetConfig<T>(string Name)
        {
            var res = GetConfig(Name);
            return (T)System.Convert.ChangeType(res, Type.GetTypeCode(typeof(T)));
        }
        public void SetConfig(string Name, string Value)
        {
            var config = base.GetList().SingleOrDefault(x => x.Title == Name);
            if (config != null)
            {
                config.Value = Value;
                base.Update(config);
            }
            else
                base.Create(new ProjectSetup { Title = Name, Value = Value });
        }
        public long DefaultSearchDuration()
        {

            if (_DefaultSearchDuration == 0)
            {
                var def = _dbContext.Set<UpdateDuration>().SingleOrDefault((x => x.IsDefault.Value));
                _DefaultSearchDuration = def.Id;
            }
            return _DefaultSearchDuration;

        }
        public int MaxDescriptionLength()
        {

            if (_MaxDesciptionLength == 0)
            {

                var res = base.GetList().SingleOrDefault(x => x.Title == "MaxDescriptionLength");
                _MaxDesciptionLength = int.Parse(res.Value.Trim());
            }
            return _MaxDesciptionLength;

        }
        public string[] RestrictSites()
        {
            string[] arr = { "facebook.com", "twitter.com", "blogspot.com", "blogger.com", "wordpress.com", "wordpress.org", "www.blogfa.com", "www.mihanblog.com" };
            return arr;
        }
        public string LuceneLocation()
        {

            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath("App_Data\\LuceneIndex");
            }
            return GetConfig("LuceneLocation");

        }
        public int TimeInterval()
        {
            return GetConfig<int>("TimeInterval");

        }//per minute
        public void StartUpDailyConfiguration()
        {

        }
        public int defCacheTimePerMin()
        {
            return 10;
        }
        public bool ChkAppPrvcy()
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
                    return val == "95.211.12.38";// && val == HttpContext.Current.Request.UserHostAddress;
            }
        }

        public OperationStatus CreateEdit(ProjectSetup model)
        {
            if (model.Id == 0)
                return base.Create(model);
            else
                return base.Update(model);
        }
        public int GetTimeInterval()
        {
            return GetConfig<int>("TimeInterval");
        }
        public DateTime GetServerNow()
        {
            return DateTime.Now.AddHours(GetConfig<float>("UTCDelay"));
        }
        public int GetVisualPostCount()
        {
            return GetConfig<int>("VisualPostCount");
        }

        public string VisualItemsPath()
        {
            return GetConfig<string>("VisualItemsPath");
        }
        public string VisualItemsPathVirtual()
        {
            return GetConfig<string>("VisualItemsPathVirtual");
        }
    }

}
