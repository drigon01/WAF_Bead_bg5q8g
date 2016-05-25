using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(WAF_Bead_bg5q8g.Startup))]
namespace WAF_Bead_bg5q8g
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
