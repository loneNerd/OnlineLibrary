using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibraryDAL.Startup))]
namespace LibraryDAL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
