using Nancy;
using Nancy.ModelBinding;
using Task.Schedu.Model;
using Task.Schedu.User;
using Task.Schedu.Utility;
using System;
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
                return Response.AsRedirect("/Login");
            };
            Post["/Info"] = x =>
            {
                var info = this.Bind<Users>();
                JsonBaseModel<Users> user = UserHelper.Login(info.UserName, info.PassWord, Request.UserHostAddress);
                if (!user.HasError && user.Result != null)
                {
                    Response.Context.Response.WithCookie(CacheKeyCollection.LoginUserId, user.Result.UserId, DateTime.Now.AddDays(2));
                    Response.Context.Response.WithCookie(CacheKeyCollection.LoginUserName, user.Result.UserName, DateTime.Now.AddDays(2));
                }
                return Response.AsJson(user);
            };
        }
    }
}