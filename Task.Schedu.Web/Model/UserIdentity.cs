using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Security;
namespace Task.Schedu.Web.Model
{
    public class UserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
