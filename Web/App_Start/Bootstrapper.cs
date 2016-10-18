using Mn.Framework.Common;
using Mn.Framework.Common.Model;
using Mn.Framework.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Common.Content;
using Tazeyab.Common.Models;
using Tazeyab.DomainClasses;
using Tazeyab.DomainClasses.ContentManagment;
using Tazeyab.DomainClasses.Logs;
using Tazeyab.DomainClasses.UpdaterBusiness;
using Tazeyab.Web.Models;
using Tazeyab.Common.Membership;
using Tazeyab.Web.Controllers;
using Tazeyab.Common.Share;
using Tazeyab.Common.ExternalService;
using Tazeyab.DomainClasses.ExternalService;
using Tazeyab.Common.Config;
using Tazeyab.DomainClasses.Config;
using Tazeyab.Common.Navigation;

namespace Tazeyab.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            ServiceFactory.Initialise(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<BaseDataContext, TazehaContext>(new PerHttpRequestLifetime());
            container.RegisterType<ISiteBusiness, SiteBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IFeedItemBusiness, FeedItemBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IFeedBusiness, FeedBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<ITagBusiness, TagBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<ICategoryBusiness, CategoryBusiness>(new HierarchicalLifetimeManager());
            //container.RegisterType<IRecentKeywordBusiness, RecentKeywordBusiness>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserBusiness, UserBusiness>(new HierarchicalLifetimeManager());           
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

            container.RegisterType(typeof(UserManager<>), new InjectionConstructor(typeof(IUserStore<>)));
            container.RegisterType<Microsoft.AspNet.Identity.IUser>(new InjectionFactory(c => c.Resolve<Microsoft.AspNet.Identity.IUser>()));
            container.RegisterType(typeof(IUserStore<>), typeof(ApplicationUserManager));
            container.RegisterType<IdentityUser<int, UserLogin, UserRole, UserClaim>, User>(new ContainerControlledLifetimeManager());
            //container.RegisterType<TazehaContext>(new ContainerControlledLifetimeManager());

            container.RegisterType<AccountController>(new InjectionConstructor());

            return container;
        }
    }
}