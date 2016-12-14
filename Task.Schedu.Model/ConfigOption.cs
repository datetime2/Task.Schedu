namespace Task.Schedu.Model
{
    /// <summary>
    /// 系统配置参数基类
    /// </summary>
    public abstract class ConfigOption
    {
        /// <summary>
        /// 当前配置项所属分组
        /// </summary>
        public virtual string GroupType { get; set; }

        /// <summary>
        /// 参数配置项保存前处理逻辑
        /// </summary>
        /// <param name="value">当前保存的参数</param>
        public virtual bool BeforeSave(OptionViewModel value)
        {
            return true;
        }

        /// <summary>
        /// 参数配置项保存后处理逻辑，一般用于重要配置项 发消息通知其他系统进行配置项更新
        /// </summary>
        /// <param name="value">当前保存的参数</param>
        public virtual void AfterSave(ConfigOption value)
        {

        }
    }
}