using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task.Schedu.Model;
using Task.Schedu.User;

namespace Task.Schedu.Web.Modules
{
    public class UserModule : BaseModule
    {
        public UserModule() : base("User")
        {
            Get["/"] = x => View["Grid"];
            Get["/Edit"] = x => View["Edit"];
            //列表查询接口
            Post["/PostQuery"] = r =>
            {
                QueryCondition condition = this.Bind<QueryCondition>();
                return Response.AsJson(UserHelper.Query(condition));
            };
        }
    }
}
