using System;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mn.NewsCms.Web.Startup))]
namespace Mn.NewsCms.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("CmsNewsContext");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            ConfigureAuth(app);

            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));
            BackgroundJob.Schedule(() => MvcApplication.Updater(), TimeSpan.FromMinutes(15));
        }
    }
}
