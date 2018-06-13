using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MediLab.Startup))]
namespace MediLab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
