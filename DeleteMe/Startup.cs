using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeleteMe.Startup))]
namespace DeleteMe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
