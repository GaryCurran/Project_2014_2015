using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShadyPines.Startup))]
namespace ShadyPines
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
