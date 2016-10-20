using Mn.Framework.Common;
using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Content;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.DomainClasses;
using Mn.NewsCms.DomainClasses.ContentManagment;
using Mn.NewsCms.DomainClasses.Logs;
using Mn.NewsCms.DomainClasses.UpdaterBusiness;
using Mn.NewsCms.Common.Membership;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Common.ExternalService;
using Mn.NewsCms.DomainClasses.ExternalService;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.DomainClasses.Config;
using Mn.NewsCms.Common.Navigation;
using Microsoft.Practices.Unity;

namespace Mn.NewsCms.UpdaterApp
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            ServiceFactory.Initialise(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<BaseDataContext, TazehaContext>(new HierarchicalLifetimeManager());
            container.RegisterType<ISiteBusiness, SiteBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IFeedItemBusiness, FeedItemBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IFeedBusiness, FeedBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<ITagBusiness, TagBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<ICategoryBusiness, CategoryBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IContactBusiness, ContactBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<ISearchHistoryBusiness, SearchHistoryBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IUpdaterDurationBusiness, UpdaterDurationBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepositorySaver, LuceneSaverRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IRecomendBiz, RecomendBiz>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogsBusiness, LogsBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IPostBiz, PostBiz>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommentBiz, CommentBiz>(new HierarchicalLifetimeManager());
            container.RegisterType<IBlogService, BlogService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAppConfigBiz, AppConfigBiz>(new HierarchicalLifetimeManager());
            container.RegisterType<IAdsBiz, AdsBiz>(new HierarchicalLifetimeManager());
            container.RegisterType<IMenuBiz, MenuBiz>(new HierarchicalLifetimeManager());


            return container;
        }
    }
}