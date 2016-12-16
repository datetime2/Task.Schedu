﻿namespace Task.Schedu.Utility
{
    /// <summary>
    /// 缓存系统所有配置信息，以键值对形式存在
    /// </summary>
    /// <remarks>
    /// 系统相关配置信息都应该通过此类的静态属性读取
    /// </remarks>
    /// <example>
    /// 获取连接字符串 SysConfig.SqlConnect
    /// </example>
    /// <summary>
    /// 系统的配置
    /// </summary>
    public class SysConfig
    {
        /// <summary>
        /// Task 调度数据库连接字符串
        /// </summary>
        [PathMap(Key = "SqlConnect")]
        public static string SqlConnect { get; set; }
        /// <summary>
        /// 业务数据库链接字符串
        /// </summary>
        [PathMap(Key = "WorkSqlConnect")]
        public static string WorkSqlConnect { get; set; }
    }
}
