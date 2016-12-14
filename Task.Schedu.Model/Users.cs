using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Schedu.Model
{
    public class Users
    {
        public string UserId { get; set; }
        public string PassWord { get; set; }
        public string PassSalt { get; set; }
        public string RealName { get; set; }
        public bool Status { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? ModifyOn { get; set; }
        public string Remark { get; set; }
        public string LastLogIP { get; set; }
        public DateTime LastLogTine { get; set; }
    }
}
