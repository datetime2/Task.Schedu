using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Task.Schedu.Jobs
{
    /// <summary>
    /// 关闭未付款订单
    /// </summary>
    public class CloseOrderWithNoPay : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
