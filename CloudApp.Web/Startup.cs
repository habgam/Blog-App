using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CloudApp.Web.Startup))]
namespace CloudApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
