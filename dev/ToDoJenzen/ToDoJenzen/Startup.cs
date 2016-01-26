using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToDoJenzen.Startup))]
namespace ToDoJenzen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
