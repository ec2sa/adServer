using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ADServerManagementWebApplication.Startup))]
namespace ADServerManagementWebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
