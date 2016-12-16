using System;
using System.ComponentModel;
using System.Text;
using Task.Schedu.Data;
using Task.Schedu.Model;
using Task.Schedu.Utility;
namespace Task.Schdeu.Message
{
    /// <summary>
    /// 消息工具类
    /// </summary>
    public class MessageHelper
    {
        /// <summary>
        /// 新增一条消息提醒
        /// </summary>
        /// <param name="Receiver">接收人（支持多个接收者逗号分隔）</param>
        /// <param name="Content">内容</param>
        /// <param name="Subject">邮件主题</param>
        /// <param name="Type">消息类型</param>
        /// <param name="FromType">消息来源类型</param>
        /// <param name="FkGUID">消息来源GUID</param>
        public static int AddMessage(string Receiver, string Content, string Subject,string FromType,string FkGUID, MessageType Type = MessageType.SMS)
        {
            if (string.IsNullOrEmpty("Receiver") || string.IsNullOrEmpty("Content"))
            {
                throw new ArgumentNullException("参数空异常");
            }
            //多个接收者逗号分隔
            string[] Receivers = Receiver.Split(new char[] { ',' });
            StringBuilder sb = new StringBuilder();
            foreach (var item in Receivers)
            {
                sb.AppendFormat(@"INSERT INTO t_Message(Receiver,Content,Subject,Type,FromType,FkGUID) SELECT '{0}',@Content,@Subject,@Type,@FromType,@FkGUID;", item);
            }
            return SQLHelper.ExecuteNonQuery(sb.ToString(), new { Content = Content, Subject = Subject, FromType = FromType, FkGUID = FkGUID,Type = EnumHelper.EnumToInt<MessageType>(Type) });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="MessageGuid">消息GUID</param>
        /// <returns>状态</returns>
        public static bool SendMessage(string MessageGuid)
        {
            Messages message = GetMessageById(MessageGuid);
            return SendMessage(message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>状态</returns>
        public static bool SendMessage(Messages message, int count = 1)
        {
            if (message == null)
            {
                throw new ArgumentNullException("参数message空异常");
            }
            bool isSuccess = false;
            SMSCode code = SMSCode.Exception;
            switch (message.Type)
            {
                //短信
                case MessageType.SMS:
                    code = SmsHelper.SendMessage(message.Receiver, message.Content);
                    break;
                case MessageType.EMAIL:
                    code = MailHelper.SendMessage(message.Receiver, message.Subject, message.Content);
                    break;
            }
            isSuccess = (code == SMSCode.Success);
            while (!isSuccess && count < 3)
            {
                //如果第一次没有发送成功再重新发送一遍
                count++;
                isSuccess = SendMessage(message, count);
            }
            RemoveMessage(message.MessageGuid, isSuccess ? "" :message.Type.GetDescription()+"重复发送三次失败,失败信息:" + code.GetDescription());
            return isSuccess;
        }

        /// <summary>
        /// 将消息移到发送历史表
        /// </summary>
        /// <param name="MessageGuid">消息GUID</param>
        /// <param name="isSuccess">是否发送成功</param>
        public static void RemoveMessage(string MessageGuid, string remark)
        {
            int Staue = string.IsNullOrEmpty(remark) ? 0 : 1;
            string strSQL = @"INSERT INTO t_MessageHistory(MessageGuid,Receiver,Type,Content,Subject,FromType,FkGUID,CreatedOn,Staue,Remark)
            SELECT  MessageGuid,Receiver,Type,Content,Subject,FromType,FkGUID,CreatedOn,@Staue,@Remark FROM t_Message WHERE MessageGuid=@MessageGuid;
            DELETE FROM t_Message WHERE MessageGuid=@MessageGuid;";
            SQLHelper.ExecuteNonQuery(strSQL, new { MessageGuid = MessageGuid, Staue = Staue, Remark = remark });
        }

        /// <summary>
        /// 通过消息GUID获取消息数据
        /// </summary>
        /// <param name="MessageGuid">消息GUID</param>
        /// <returns>消息</returns>
        public static Messages GetMessageById(string MessageGuid)
        {
            return SQLHelper.Single<Messages>("SELECT * FROM t_Message WHERE MessageGuid=@MessageGuid", new { MessageGuid = MessageGuid });
        }
    }


}
