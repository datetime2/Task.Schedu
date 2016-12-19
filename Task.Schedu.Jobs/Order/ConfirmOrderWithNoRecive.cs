using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Schedu.Data;

namespace Task.Schedu.Jobs
{
    /// <summary>
    /// 未收货订单 自动收货
    /// </summary>
    public class ConfirmOrderWithNoRecive : DbAccess<dynamic>, IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
