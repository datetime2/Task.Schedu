using Task.Schedu.Jobs.Utils;
using Task.Schedu.Utility;
using Quartz;
using System;
using System.Collections.Generic;
using Task.Schedu.Data;
using Task.Schdeu.Message;
using Task.Schedu.Model;

namespace Task.Schedu.Jobs
{
    /// <summary>
    /// 发送消息任务
    /// </summary>
    [DisallowConcurrentExecution]
    public class SendMessageJob : IJob
    {
        /// 取出t_Message表里面所有数据进行发送
        /// </summary>
        private static readonly string strSQL2 = @"SELECT MessageGuid,Receiver,Content,Subject,Type,CreatedOn FROM t_Message ";
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime start = DateTime.Now;
                TaskLog.SendMessageLogInfo.WriteLogE("\r\n\r\n\r\n\r\n------------------发送信息任务开始执行 " + start.ToString("yyyy-MM-dd HH:mm:ss") + " BEGIN-----------------------------\r\n\r\n");

                //取出所有当前待发送的消息
                List<Messages> listWait = SQLHelper.ToList<Messages>(strSQL2);
                bool isSucess = false;
                if (listWait == null || listWait.Count == 0)
                {
                    TaskLog.SendMessageLogInfo.WriteLogE("当前没有等待发送的消息!");
                }
                else
                {
                    foreach (var item in listWait)
                    {
                        isSucess = MessageHelper.SendMessage(item);
                        TaskLog.SendMessageLogInfo.WriteLogE(string.Format("接收人:{0},类型:{1},内容:“{2}”的消息发送{3}", item.Receiver, item.Type.ToString(), item.Content, isSucess ? "成功" : "失败"));
                    }
                }
                DateTime end = DateTime.Now;
                TaskLog.SendMessageLogInfo.WriteLogE("\r\n\r\n------------------发送信息任务完成:" + end.ToString("yyyy-MM-dd HH:mm:ss") + ",本次共耗时(分):" + (end - start).TotalMinutes + " END------------------------\r\n\r\n\r\n\r\n");
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                TaskLog.SendMessageLogError.WriteLogE("发送信息任务异常", ex);
                //1.立即重新执行任务 
                e2.RefireImmediately = true;
            }
        }
    }
}
