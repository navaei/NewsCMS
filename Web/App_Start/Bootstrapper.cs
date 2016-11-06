using Mn.NewsCms.Common.BaseClass;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Content;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.DomainClasses;
using Mn.NewsCms.DomainClasses.ContentManagment;
using Mn.NewsCms.DomainClasses.Logs;
using Mn.NewsCms.DomainClasses.UpdaterBusiness;
using Mn.NewsCms.Common.Membership;
using Mn.NewsCms.Web.Controllers;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Common.ExternalService;
using Mn.NewsCms.DomainClasses.ExternalService;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.DomainClasses.Config;
using Mn.NewsCms.Common.Navigation;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web
{
    public static class Bootstrapper
    {
        private static IUnityContainer container;
        public static void Initialise()
        {
            container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            ServiceFactory.Initialize(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IUnitOfWork, TazehaContext>(new PerHttpRequestLifetime());
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

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}