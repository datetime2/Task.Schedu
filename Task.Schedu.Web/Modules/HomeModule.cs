using Nancy;
using Task.Schedu.Model;

namespace Task.Schedu.Web.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule():base("Home")
        {
            Get["/"] = r =>
            {
                var model = SystemConfig.SystemTitle;
                return View["Index", model];
            };
        }
    }
}