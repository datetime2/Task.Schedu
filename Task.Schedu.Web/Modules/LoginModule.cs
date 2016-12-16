using Nancy;
using Nancy.ModelBinding;
using Task.Schedu.Model;
using Task.Schedu.User;
using Task.Schedu.Utility;
using System;
using Nancy.Authentication.Forms;
using Nancy.Security;
using Task.Schedu.Web.Model;

namespace Task.Schedu.Web.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
            Get["/Login"] = r =>
            {
                var model = SystemConfig.SystemTitle;
                return View["Index", model];
            };
            Get["/Login/Exit"] = r =>
            {
               return this.LogoutAndRedirect("/Login");
            };
            Post["/Login/Info"] = x =>
            {
                var info = this.Bind<Users>();
                JsonBaseModel<Users> user = UserHelper.Login(info.UserName, info.PassWord, Request.UserHostAddress);
                if (!user.HasError && user.Result != null)
                    return this.LoginAndRedirect(Guid.Parse(user.Result.UserId), cookieExpiry: DateTime.Now.AddMinutes(2), fallbackRedirectUrl: "/Home");
                else
                    return Response.AsJson(user);
            };
        }
    }

    public class UserMapper : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = UserHelper.GetById(identifier.ToString());
            if (user == null) return null;
            else
                return new UserIdentity
                {
                    UserName = user.UserName,
                    Claims = new[]
                    {
                        user.RealName
                    }
                };
        }
    }
}