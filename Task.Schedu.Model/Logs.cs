using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Schedu.Model
{
    /// <summary>
    /// Task 调度日志记录
    /// </summary>
    public class Logs
    {
        public long LogId { get; set; }
        public LogType LogType { get; set; }
        public string LogMsg { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    /// <summary>
    /// 日至等级
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// INFO 
        /// </summary>
        INFO,
        /// <summary>
        /// DEBUG
        /// </summary>
        DEBUG,
        /// <summary>
        /// ERROR
        /// </summary>
        ERROR
    }
}
