using Task.Schedu.Utility;

namespace Task.Schedu.Jobs.Utils
{
    /// <summary>
    /// 获取任务记录日志ILog
    /// </summary>
    public class TaskLog
    {
        /// <summary>
        /// 发送消息任务普通日志
        /// </summary>
        public static LogHelper SendMessageLogInfo = new LogHelper("SendMessageJob", "info");

        /// <summary>
        /// 发送消息任务异常日志
        /// </summary>
        public static LogHelper SendMessageLogError = new LogHelper("SendMessageJob", "error");

        /// <summary>
        /// 配置任务普通日志
        /// </summary>
        public static LogHelper ConfigLogInfo = new LogHelper("ConfigJob", "info");

        /// <summary>
        /// 配置任务异常日志
        /// </summary>
        public static LogHelper ConfigLogError = new LogHelper("ConfigJob", "error");

        /// <summary>
        /// 配置任务普通日志
        /// </summary>
        public static LogHelper AutoRunLogInfo = new LogHelper("AutoRunJob", "info");

        /// <summary>
        /// 配置任务异常日志
        /// </summary>
        public static LogHelper AutoRunLogError = new LogHelper("AutoRunJob", "error");

        /// <summary>
        /// 订单普通日志
        /// </summary>
        public static LogHelper OrderNoPayCloseLogInfo = new LogHelper("OrderNoPayCloseJob", "info");

        /// <summary>
        /// 订单异常日志
        /// </summary>
        public static LogHelper OrderNoPayCloseLogError = new LogHelper("OrderNoPayCloseJob", "error");

    }
}
