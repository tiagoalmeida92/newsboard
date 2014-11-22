using Microsoft.Owin;
using NewsBoard.Web;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace NewsBoard.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}