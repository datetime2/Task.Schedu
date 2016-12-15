using Nancy;
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
            Before += ctx => 
            {
                //静态资源版本
                ViewBag.Version = SystemConfig.StaticVersion;
                return null;
            };
        }
    }
}