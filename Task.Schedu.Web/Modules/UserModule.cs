using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task.Schedu.Access;
using Task.Schedu.Model;
using Task.Schedu.Utility;

namespace Task.Schedu.Web.Modules
{
    public class UserModule : BaseModule
    {
        public UserModule() : base("User")
        {
            Get["/"] = x => View["Grid"];
            Get["/Edit"] = x => View["Edit"];
            Post["/PostQuery"] = r =>
            {
                QueryCondition condition = this.Bind<QueryCondition>();
                return Response.AsJson(UserAccess.Query(condition));
            };
            Put["/{Id}/{Status:int}"] = r =>
            {
                JsonBaseModel<string> result = new JsonBaseModel<string>();
                try
                {
                    UserAccess.UpdUserStatus(r.Id, r.Status);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };
            Delete["/{Id}"] = r =>
            {
                JsonBaseModel<string> result = new JsonBaseModel<string>();
                try
                {
                    string TaskId = r.Id;
                    UserAccess.DeleteById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };
            Post["/"] = r =>
            {
                var user = this.Bind<Users>();
                return Response.AsJson(UserAccess.SaveUser(user));
            };
            Put["/"] = r =>
            {
                var user = this.Bind<Users>();
                return Response.AsJson(UserAccess.SaveUser(user));
            };
            Get["/GetById/{Id}"] = r =>
            {
                JsonBaseModel<Users> result = new JsonBaseModel<Users>();
                try
                {
                    //取出单条记录数据
                    string TaskId = r.Id;
                    result.Result = UserAccess.GetById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };
        }
    }
}
