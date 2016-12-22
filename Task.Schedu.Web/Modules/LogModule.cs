using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.ModelBinding;
using Task.Schedu.Model;
using Task.Schedu.Access;

namespace Task.Schedu.Web.Modules
{
    public class LogModule:BaseModule
    {
        public LogModule():base("Log")
        {
            Get["/"] = x => View["Grid"];

            Post["/PostQuery"] = r =>
            {
                QueryCondition condition = this.Bind<QueryCondition>();
                return Response.AsJson(LogAccess.Query(condition));
            };
        }
    }
}
