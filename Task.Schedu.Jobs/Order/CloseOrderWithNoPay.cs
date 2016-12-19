using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Schedu.Data;
using Task.Schedu.Utility;
using Dapper;
using Task.Schedu.Jobs.Utils;

namespace Task.Schedu.Jobs
{
    /// <summary>
    /// 关闭未付款订单
    /// </summary>
    [DisallowConcurrentExecution]
    public class CloseOrderWithNoPay : DbAccess<dynamic>, IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //TaskLog.OrderNoPayCloseLogInfo.WriteLogE("开始订单关闭操作");
            TaskLog.OrderNoPayCloseLogInfo.WriteLogE(SysConfig.MainConnect);
            var orderUser = FindBy((client) =>
             {
                 return client.Query("SEELCT OrderId,UserId FROM Orders LIMIT 0,100");
             }, SysConfig.MainConnect);
            if (orderUser.Any())
            {
                TaskLog.OrderNoPayCloseLogInfo.WriteLogE("查询订单集合操作");
                var flag = Commit((client) =>
                  {
                      return client.Execute("UPDATE Orders SET OrderStatus=0 WHERE OrderId=@OrderId", new { OrderId = orderUser.Select(s => s.OrderId) }) > 0;
                  }, SysConfig.MainConnect);
            }
            TaskLog.OrderNoPayCloseLogInfo.WriteLogE("结束订单关闭操作");
        }
    }
}
