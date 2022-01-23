using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UploadTransaction_System.Startup))]
namespace UploadTransaction_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
