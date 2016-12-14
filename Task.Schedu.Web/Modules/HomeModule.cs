using Nancy;
using Task.Schedu.Utility.ConfigHandler;

namespace Task.Schedu.Web.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule()
        {
            //主页
            Get["/"] = r =>
            {
                return Response.AsRedirect("/Home/Index");
            };
            //主页
            Get["/Home/Index"] = r =>
            {
                var model = SystemConfig.SystemTitle;
                return View["Index", model];
            };
        }
    }
}