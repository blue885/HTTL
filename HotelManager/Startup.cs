using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HotelManager.Startup))]
namespace HotelManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
