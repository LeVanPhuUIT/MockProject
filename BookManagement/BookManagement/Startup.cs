using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookManagement.Startup))]
namespace BookManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
