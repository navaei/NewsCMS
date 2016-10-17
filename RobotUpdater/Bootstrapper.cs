using Mn.Framework.Common;
using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Content;
using Tazeyab.Common.Models;
using Tazeyab.DomainClasses;
using Tazeyab.DomainClasses.ContentManagment;
using Tazeyab.DomainClasses.Logs;
using Tazeyab.DomainClasses.UpdaterBusiness;
using Tazeyab.Common.Membership;
using Tazeyab.Common.Share;
using Tazeyab.Common.ExternalService;
using Tazeyab.DomainClasses.ExternalService;
using Tazeyab.Common.Config;
using Tazeyab.DomainClasses.Config;
using Tazeyab.Common.Navigation;
using Microsoft.Practices.Unity;

namespace Tazeyab.Robot.Updater
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
            container.RegisterType<IRwpBiz, RwpBiz>(new HierarchicalLifetimeManager());
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