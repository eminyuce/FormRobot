using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FormRobot.Startup))]
namespace FormRobot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            
            ConfigureAuth(app);
        }
    }
}
