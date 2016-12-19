using Nancy;
using Nancy.Security;
using Task.Schedu.Model;
namespace Task.Schedu.Web.Modules
{
    public class BaseModule: NancyModule
    {
        public BaseModule()
        {
            Init();
        }

        public BaseModule(string modulePath): base(modulePath)
        {
            Init();
        }
        private void Init()
        {
            //this.RequiresAuthentication();
            Before += ctx =>
            {
                ViewBag.Version = SystemConfig.StaticVersion;
                return null;
            };
        }
    }
}