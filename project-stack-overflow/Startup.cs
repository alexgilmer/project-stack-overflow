using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(project_stack_overflow.Startup))]
namespace project_stack_overflow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
