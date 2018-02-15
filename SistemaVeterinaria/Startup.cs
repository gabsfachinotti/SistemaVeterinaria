using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaVeterinaria.Startup))]
namespace SistemaVeterinaria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
