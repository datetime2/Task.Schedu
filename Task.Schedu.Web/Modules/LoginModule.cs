using Nancy;
namespace Task.Schedu.Web.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule() : base("Login")
        {
            Get["/"] = r =>
            {
                var model = "我是 Razor 引擎2";
                return View["Index", model];
            };

            //退出登录
            Get["/Exit"] = r =>
            {
                var model = "我是 Razor 引擎";
                return View["Exit", model];
            };
        }
    }
}