using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.Schedu.Data;
using Task.Schedu.Utility;
using Task.Schedu.Testing.Utils;
using Dapper;
using System.Linq;
namespace Task.Schedu.Testing
{
    /// <summary>
    /// 订单模块单元测试
    /// </summary>
    [TestClass]
    public class OrderTest : DbAccess<dynamic>
    {
        public OrderTest()
        {
            ConfigInit.Init();
        }


        #region 逾期未付款订单关闭
        [TestMethod]
        public void OrderAutoClose()
        {
            TaskLog.OrderNoPayCloseLogInfo.WriteLogE("开始订单关闭操作");
            //待处理订单
            var orderUser = FindBy((client) =>
            {
                return client.Query("SELECT Id,UserId,UserName,IntegralDiscount,PurseAmount FROM Orders LIMIT 0,100");

            }, SysConfig.MainConnect);
            //开始处理
            if (orderUser.Any())
            {
                var flag = Commit((client) =>
                {
                    foreach (var item in orderUser)
                    {
                        try
                        {
                            client.Execute("UPDATE Orders SET OrderStatus=5,FinishDate=@FinishDate,CloseReason=@CloseReason WHERE Id=@OrderId", new { OrderId = item.Id, FinishDate = DateTime.Now, CloseReason = "逾期未付款,自动关闭" });
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    return client.Execute("UPDATE Orders SET OrderStatus=10 WHERE Id=@OrderId", new { OrderId = orderUser.Select(s => s.Id) }) > 0;
                }, SysConfig.MainConnect);
            }
            TaskLog.OrderNoPayCloseLogInfo.WriteLogE("结束订单关闭操作");
        }
        #endregion

        #region 逾期未确认订单确认
        [TestMethod]
        public void OrderAutoConfirm()
        {
        }
        #endregion


    }
}
