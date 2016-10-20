using System;
using System.Linq;
using Mn.NewsCms.Common.Models;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

namespace Mn.NewsCms.Common.Config
{
    public interface IAppConfigBiz
    {
        IQueryable<ProjectSetup> GetList();
        OperationStatus CreateEdit(ProjectSetup model);
        string GetConfig(string Name);
        T GetConfig<T>(string Name);
        void SetConfig(string Name, string Value);
        long DefaultSearchDuration();
        int MaxDescriptionLength();
        string[] RestrictSites();
        string LuceneLocation();
        string VisualItemsPath();
        string VisualItemsPathVirtual();
        void StartUpDailyConfiguration();
        int defCacheTimePerMin();
        bool ChkAppPrvcy();
        int GetTimeInterval();
        int GetVisualPostCount();
        DateTime GetServerNow();
    }
}
