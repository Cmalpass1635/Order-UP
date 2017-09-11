using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrderUp.Startup))]
namespace OrderUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
