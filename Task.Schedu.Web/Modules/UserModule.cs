using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task.Schedu.Model;
using Task.Schedu.User;
using Task.Schedu.Utility;

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
            //更新用户状态
            Put["/{Id}/{Status:int}"] = r =>
            {
                JsonBaseModel<string> result = new JsonBaseModel<string>();
                try
                {
                    UserHelper.UpdUserStatus(r.Id, r.Status);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };
            //
            Delete["/{Id}"] = r =>
            {
                JsonBaseModel<string> result = new JsonBaseModel<string>();
                try
                {
                    string TaskId = r.Id;
                    UserHelper.DeleteById(TaskId);
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
