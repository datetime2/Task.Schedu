using System.Collections.Generic;
using System.ComponentModel.Composition;
using Task.Schedu.Data;
using Task.Schedu.Model;

namespace Task.Schedu.ConfigHandler
{
    /// <summary>
    /// 系统配置接口
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// 获取所有配置项
        /// </summary>
        /// <returns>所有配置项</returns>
        List<Options> GetAllOptions(string GroupType = "");
    }

    [Export(typeof(IConfigService))]
    public class ConfigService : IConfigService
    {
        /// <summary>
        /// 获取所有配置项
        /// </summary>
        /// <returns>所有配置项</returns>
        public List<Options> GetAllOptions(string GroupType = "")
        {
            string strSQL = "select * from t_Configuration";
            if (!string.IsNullOrEmpty(GroupType))
            {
                strSQL += string.Format(" where OPTIONTYPE='{0}'", GroupType);
            }
            return SQLHelper.ToList<Options>(strSQL);
        }
    }
}