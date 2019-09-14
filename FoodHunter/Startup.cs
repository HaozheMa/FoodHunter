using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FoodHunter.Startup))]
namespace FoodHunter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
