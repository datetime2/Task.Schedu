using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Task.Schedu.Jobs.Utils;

namespace Task.Schedu.Jobs
{
    [DisallowConcurrentExecution]
    public class OrderNoPayCloseJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            TaskLog.OrderNoPayCloseLogInfo.WriteLogE("开始订单关闭操作");
        }
    }
}
