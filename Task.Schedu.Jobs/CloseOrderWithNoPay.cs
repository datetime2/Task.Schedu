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
using Task.Schedu.Model;

namespace Task.Schedu.Jobs
{
    /// <summary>
    /// 关闭未付款订单
    /// </summary>
    [DisallowConcurrentExecution]
    public class CloseOrderWithNoPay : DbAccess<dynamic>, IJob
    {
        public CloseOrderWithNoPay()
        {
            ConfigInit.Init();
        }
        public void Execute(IJobExecutionContext context)
        {
            var logs = new List<Logs>();
            logs.Add(new Logs
            {
                LogId = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.Now,
                LogType = LogType.DEBUG,
                LogMsg = "逾期未付款订单关闭开始......"
            });
            //待处理订单
            var NoPayOrders = FindBy((client) =>
            {
                Orders orders = null;
                return client.Query<Orders, OrderItems, Orders>(@"SELECT A.Id,A.UserId,A.UserName,A.IntegralDiscount,A.PurseAmount,B.OrderId,B.SkuId,B.Quantity FROM Orders A  
                    Left Join OrderItems B ON A.Id=B.OrderId WHERE OrderStatus=1 And OrderDate<@OrderDate LIMIT 0,100",
                    (order, item) =>
                    {
                        if (orders == null || orders.Id != order.Id)
                            orders = order;
                        if (item != null)
                        {
                            orders.OrderItems.Add(item);
                        }
                        return orders;
                    }, new { OrderDate = DateTime.Now.AddMinutes(-30) }, splitOn: "OrderId");
            }, SysConfig.MainConnect);


            TaskLog.OrderNoPayCloseLogInfo.WriteLogE(NoPayOrders.Count() + "");

            //开始处理
            if (NoPayOrders.Any())
            {
                Commit((client) =>
                {
                    foreach (var item in NoPayOrders)
                    {
                        try
                        {
                            //修改订单状态
                            var flag = client.Execute("UPDATE Orders SET OrderStatus=5,FinishDate=@FinishDate,CloseReason=@CloseReason WHERE Id=@OrderId",
                                  new
                                  {
                                      OrderId = item.Id,
                                      FinishDate = DateTime.Now,
                                      CloseReason = "逾期未付款,自动关闭"
                                  }) > 0;

                            if (flag)
                            {
                                //返还库存
                                foreach (var items in item.OrderItems)
                                {
                                    client.Execute("UPDATE SKUs SET Stock=Stock+@Quantity,MaxSaleStock=MaxSaleStock+Quantity WHERE Id=@SkuId", new { items.Quantity, items.SkuId });
                                }
                                //返还优惠券
                                //var couponRecord = client.Query("SELECT CounponStatus,UserId,UsedTime,OrderId,CouponPackageId FROM CouponRecord WHERE ")
                                //返还钱包金额
                                logs.Add(new Logs
                                {
                                    LogId = Guid.NewGuid().ToString(),
                                    CreatedOn = DateTime.Now,
                                    LogType = LogType.INFO,
                                    LogMsg = string.Format("订单{0}处理成功", item.Id)
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            logs.Add(new Logs
                            {
                                LogId = Guid.NewGuid().ToString(),
                                CreatedOn = DateTime.Now,
                                LogType = LogType.ERROR,
                                LogMsg = string.Format("订单{0}处理异常,{1}", item.Id, ex.Message)
                            });
                            continue;
                        }
                    }
                    return true;
                }, SysConfig.MainConnect);
            }
            logs.Add(new Logs
            {
                LogId = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.Now,
                LogType = LogType.DEBUG,
                LogMsg = "逾期未付款订单关闭结束......"
            });
            //订单操作异常日志
            if (logs.Any())
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    Commit((client) =>
                    {
                        client.Execute(@"INSERT INTO t_OrderLog(LogId,LogType,LogMsg,CreatedOn) 
                                                 VALUES(@LogId,@LogType,@LogMsg,@CreatedOn)", logs);
                        return true;
                    }, SysConfig.ScheduConnect);
                });
            }
        }
    }
}
