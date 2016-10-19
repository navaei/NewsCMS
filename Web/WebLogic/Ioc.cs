using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.Content;
using Mn.NewsCms.Common.ExternalService;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Navigation;
using Mn.NewsCms.Common.Share;

namespace Mn.NewsCms.Web
{
    public static class Ioc
    {
        public static TazehaContext DataContext
        {
            get
            {
                return (TazehaContext)DependencyResolver.Current.GetService(typeof(BaseDataContext));
            }
        }
        public static ISiteBusiness SiteBiz
        {
            get
            {
                return (ISiteBusiness)DependencyResolver.Current.GetService(typeof(ISiteBusiness));
            }
        }
        public static ITagBusiness TagBiz
        {
            get
            {
                return (ITagBusiness)DependencyResolver.Current.GetService(typeof(ITagBusiness));
            }
        }
        //public static IRecentKeywordBusiness RecentKeywordBiz
        //{
        //    get
        //    {
        //        return (IRecentKeywordBusiness)DependencyResolver.Current.GetService(typeof(IRecentKeywordBusiness));
        //    }
        //}
        public static ICategoryBusiness CatBiz
        {
            get
            {
                return (ICategoryBusiness)DependencyResolver.Current.GetService(typeof(ICategoryBusiness));
            }
        }
        public static IFeedBusiness FeedBiz
        {
            get
            {
                return (IFeedBusiness)DependencyResolver.Current.GetService(typeof(IFeedBusiness));
            }
        }
        public static IFeedItemBusiness ItemBiz
        {
            get
            {
                return (IFeedItemBusiness)DependencyResolver.Current.GetService(typeof(IFeedItemBusiness));
            }
        }
        public static IUserBusiness UserBiz
        {
            get
            {
                return (IUserBusiness)DependencyResolver.Current.GetService(typeof(IUserBusiness));
            }
        }     
        public static IContactBusiness ContactBiz
        {
            get
            {
                return (IContactBusiness)DependencyResolver.Current.GetService(typeof(IContactBusiness));
            }
        }
        public static ISearchHistoryBusiness SearchHistoryBiz
        {
            get
            {
                return (ISearchHistoryBusiness)DependencyResolver.Current.GetService(typeof(ISearchHistoryBusiness));
            }
        }
        public static IRecomendBiz RecomendBiz
        {
            get
            {
                return (IRecomendBiz)DependencyResolver.Current.GetService(typeof(IRecomendBiz));
            }
        }
        public static ILogsBusiness LogBiz
        {
            get
            {
                return (ILogsBusiness)DependencyResolver.Current.GetService(typeof(ILogsBusiness));
            }
        }
        public static IPostBiz PostBiz
        {
            get
            {
                return (IPostBiz)DependencyResolver.Current.GetService(typeof(IPostBiz));
            }
        }
        public static ICommentBiz CommentBiz
        {
            get
            {
                return (ICommentBiz)DependencyResolver.Current.GetService(typeof(ICommentBiz));
            }
        }
        public static IBlogService BlogService
        {
            get
            {
                return (IBlogService)DependencyResolver.Current.GetService(typeof(IBlogService));
            }
        }
        public static IAppConfigBiz AppConfigBiz
        {
            get
            {
                return (IAppConfigBiz)DependencyResolver.Current.GetService(typeof(IAppConfigBiz));
            }
        }
        public static IAdsBiz AdsBiz
        {
            get
            {
                return (IAdsBiz)DependencyResolver.Current.GetService(typeof(IAdsBiz));
            }
        }
        public static IUpdaterDurationBusiness UpdaterDurationBiz
        {
            get
            {
                return (IUpdaterDurationBusiness)DependencyResolver.Current.GetService(typeof(IUpdaterDurationBusiness));
            }
        }
        public static IMenuBiz MenuBiz
        {
            get
            {
                return (IMenuBiz)DependencyResolver.Current.GetService(typeof(IMenuBiz));
            }
        }
    }
}