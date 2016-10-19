using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mn.NewsCms.Web.Startup))]
namespace Mn.NewsCms.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
