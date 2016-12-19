using Task.Schedu.Model;
namespace Task.Schedu.Web.Modules
{
    public class HomeModule : BaseModule
    {
        public HomeModule():base("Home")
        {
            Get["/"] = r =>
            {
                var model = new
                {
                    Title = SystemConfig.SystemTitle,
                    CurrentUser = "Admin"//Context.CurrentUser.UserName
                };
                return View["Index", model];
            };
        }
    }
}