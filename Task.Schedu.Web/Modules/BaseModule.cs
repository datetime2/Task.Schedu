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
                ViewBag.Version = SystemConfig.StaticVersion;
                //if (1 == 1)
                //    return HttpStatusCode.Unauthorized;
                //else
                    return null;
            };
        }
    }
}