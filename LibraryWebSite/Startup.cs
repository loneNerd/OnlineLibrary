using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryWebSite.Startup))]
namespace LibraryWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
