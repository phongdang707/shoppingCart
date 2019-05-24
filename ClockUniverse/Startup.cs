using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClockUniverse.Startup))]
namespace ClockUniverse
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
