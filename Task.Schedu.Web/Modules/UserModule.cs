using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Schedu.Web.Modules
{
    public class UserModule : BaseModule
    {
        public UserModule() : base("User")
        {
            Get["/"] = x => View["Grid"];
        }
    }
}
