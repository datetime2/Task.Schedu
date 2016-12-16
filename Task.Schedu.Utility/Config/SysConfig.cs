namespace Task.Schedu.Utility
{
    /// <summary>
    /// 缓存系统所有配置信息，以键值对形式存在
    /// </summary>
    /// <remarks>
    /// 系统相关配置信息都应该通过此类的静态属性读取
    /// </remarks>
    /// <example>
    /// 获取连接字符串 SysConfig.ScheduConnect
    /// </example>
    /// <summary>
    /// 系统的配置
    /// </summary>
    public class SysConfig
    {
        /// <summary>
        /// Task调度数据库连接字符串
        /// </summary>
        [PathMap(Key = "ScheduConnect")]
        public static string ScheduConnect { get; set; }
        /// <summary>
        /// 评论业务数据库链接字符串
        /// </summary>
        [PathMap(Key = "CommentConnect")]
        public static string CommentConnect { get; set; }
        /// <summary>
        /// 会员业务数据库链接字符串
        /// </summary>
        [PathMap(Key = "MemberConnect")]
        public static string MemberConnect { get; set; }
        /// <summary>
        /// 配置业务数据库链接字符串
        /// </summary>
        [PathMap(Key = "SettingConnect")]
        public static string SettingConnect { get; set; }
    }
}
