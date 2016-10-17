using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tazeyab.Web.Startup))]
namespace Tazeyab.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
