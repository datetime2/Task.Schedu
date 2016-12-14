using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Task.Schedu.Model
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// 消息GUID
        /// </summary>
        public string MessageGuid { get; set; }

        /// <summary>
        /// 消息接收人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// 消息创建日期
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 消息来源类型
        /// </summary>
        public string FromType { get; set; }

        /// <summary>
        /// 消息来源GUID
        /// </summary>
        public Guid FkGUID { get; set; }
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 短信消息
        /// </summary>
        [Description("短信消息")]
        SMS = 0,
        /// <summary>
        /// 邮件消息
        /// </summary>
        [Description("邮件消息")]
        EMAIL = 1
    }
}
