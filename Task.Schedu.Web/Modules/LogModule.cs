using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Schedu.Web.Modules
{
    public class LogModule:BaseModule
    {
        public LogModule():base("Log")
        {
            Get["/"] = x => View["Grid"];
        }
    }
}
