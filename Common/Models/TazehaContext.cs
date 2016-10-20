using System.Data.Entity;
using Mn.NewsCms.Common.Membership;
using Mn.NewsCms.Common.Models.Mapping;
using Mn.NewsCms.Common.Navigation;
using BaseDataContext = Mn.NewsCms.Common.BaseClass.BaseDataContext;

namespace Mn.NewsCms.Common.Models
{
    public class TazehaContext : BaseDataContext
    {
        public TazehaContext()
            : base("Name=TazehaContext")
        {
            //Database.SetInitializer<TazehaContext>(null);
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        // Account tables
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Role> Roles { get; set; }
        public virtual IDbSet<UserClaim> Claims { get; set; }

        public DbSet<Ad> Ads { get; set; }
        //public DbSet<AdsContainer> AdsContainers { get; set; }
        //public DbSet<Application> Applications { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        //public DbSet<Crawler_Log> Crawler_Log { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        //public DbSet<FeedItems_Index> FeedItems_Index { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        //public DbSet<ImportanceRate> ImportanceRates { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<LogsBuffer> LogsBuffers { get; set; }       
        public DbSet<PhotoItem> PhotoItems { get; set; }
        public DbSet<ProjectSetup> ProjectSetups { get; set; }
        public DbSet<RecentKeyWord> RecentKeyWords { get; set; }
        public DbSet<RelatedSite> RelatedSites { get; set; }
        public DbSet<RemoteRequestLog> RemoteRequestLogs { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SocialTracker> SocialTrackers { get; set; }
        //public DbSet<Sponser> Sponsers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        //public DbSet<Tags_Temp> Tags_Temp { get; set; }
        public DbSet<UpdateDuration> UpdateDurations { get; set; }
        //public DbSet<UselessWord> UselessWords { get; set; }
        //public DbSet<ValueRefrence> ValueRefrences { get; set; }
        public DbSet<WebPart> WebParts { get; set; }
        public DbSet<WebPartsContainer> WebPartsContainers { get; set; }
        public DbSet<ItemVisited> ItemVisiteds { get; set; }
        public DbSet<NewsletterUser> NewsletterUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<FeedLog> FeedLogs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AdMap());
            //modelBuilder.Configurations.Add(new AdsContainerMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new ContactMessageMap());          
            modelBuilder.Configurations.Add(new FeedItemMap());
            //modelBuilder.Configurations.Add(new FeedItems_IndexMap());
            modelBuilder.Configurations.Add(new FeedMap());
            //modelBuilder.Configurations.Add(new ImportanceRateMap());
            modelBuilder.Configurations.Add(new KeyMap());
            modelBuilder.Configurations.Add(new LogsBufferMap());         
            modelBuilder.Configurations.Add(new PhotoItemMap());
            modelBuilder.Configurations.Add(new ProjectSetupMap());
            modelBuilder.Configurations.Add(new RecentKeyWordMap());
            modelBuilder.Configurations.Add(new RelatedSiteMap());
            modelBuilder.Configurations.Add(new RemoteRequestLogMap());
            modelBuilder.Configurations.Add(new SiteMap());
            modelBuilder.Configurations.Add(new SocialTrackerMap());
            modelBuilder.Configurations.Add(new SponserMap());
            modelBuilder.Configurations.Add(new TagMap());
            //modelBuilder.Configurations.Add(new Tags_TempMap());
            modelBuilder.Configurations.Add(new UpdateDurationMap());
            //modelBuilder.Configurations.Add(new UselessWordMap());
            //modelBuilder.Configurations.Add(new ValueRefrenceMap());
            modelBuilder.Configurations.Add(new WebPartMap());
            modelBuilder.Configurations.Add(new ItemVisitedMap());
            modelBuilder.Configurations.Add(new WebPartsContainerMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new MenuItemMap());
            modelBuilder.Configurations.Add(new FeedLogMap());

            modelBuilder.Entity<User>().HasKey<int>(l => l.Id).Map(map => map.ToTable("AppUsers"));
            modelBuilder.Entity<User>().HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserClaim>().HasKey(l => l.Id).Map(map => map.ToTable("AppUserClaims"));
            modelBuilder.Entity<UserLogin>().HasKey<int>(l => l.UserId).Map(map => map.ToTable("AppUserLogins"));
            modelBuilder.Entity<Role>().HasKey<int>(r => r.Id).Map(map => map.ToTable("AppRoles"));
            modelBuilder.Entity<UserRole>().HasKey(r => new { r.RoleId, r.UserId }).Map(map => map.ToTable("AppUserRoles"));
        }

        public static TazehaContext Create()
        {
            return new TazehaContext();
        }

    }
}
