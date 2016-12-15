using Nancy;
using Nancy.ModelBinding;
using Task.Schedu.Model;
using Task.Schedu.User;
using Task.Schedu.Utility;

namespace Task.Schedu.Web.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule() : base("Login")
        {
            Get["/"] = r =>
            {
                var model = SystemConfig.SystemTitle;
                return View["Index", model];
            };
            Get["/Exit"] = r =>
            {
                var model = "我是 Razor 引擎";
                return View["Exit", model];
            };
            Post["/Info"] = x =>
            {
                var info = this.Bind<Users>();
                JsonBaseModel<Users> user = UserHelper.Login(info.UserName, info.PassWord);
                return Response.AsJson(user);
            };
        }
    }
}