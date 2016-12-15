using Nancy.Hosting.Self;
using System;
using System.Net;
using Task.Schedu.ConfigHandler;
using Task.Schedu.Model;
using Task.Schedu.Utility;

namespace Owin_Nancy
{
    public class Startup
    {
        private static NancyHost _host = null;
        /// <summary>
        /// 监听端口 启动站点
        /// </summary>
        /// <param name="urls">监听ip端口列表</param>
        public static NancyHost Start(string host = "127.0.0.1", int port = 80)
        {
            try
            {
                _host = new NancyHost(new Uri(string.Format("http://{1}:{0}", port,host)));
                _host.Start();
                LogHelper.WriteLog(string.Format("Web管理站点启动成功,请打开 http://{0}:{1}进行浏览", host, port));
                if (SystemConfig.WebPort != port || !SystemConfig.WebHost.Equals(host))
                {
                    //更新系统参数配置表监听端口
                    SystemConfig.WebPort = port;
                    SystemConfig.WebHost = host;
                    ConfigManager.UpdateValueByKey("SystemConfig", "WebPort", port.ToString());
                    ConfigManager.UpdateValueByKey("SystemConfig", "WebHost", host);
                }
                return _host;
            }
            catch (HttpListenerException ex)
            {
                LogHelper.WriteLog("Web管理站点启动失败", ex);
                Random random = new Random();
                port = random.Next(port - 1000, port + 1000);
                Console.WriteLine(ex.Message);
                Console.WriteLine(" 重新尝试端口:" + port);
                return Start(host,port);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Web管理站点启动失败", ex);
                throw ex;
            }
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        public static void Dispose()
        {
            //回收资源
            if (_host != null)
            {
                _host.Stop();
                _host.Dispose();
                _host = null;
            }
        }
    }
}
