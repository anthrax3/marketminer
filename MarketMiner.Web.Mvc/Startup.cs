using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MarketMiner.Web.Mvc.Startup))]
namespace MarketMiner.Web.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           ConfigureAuth(app);
        }
    }
}
