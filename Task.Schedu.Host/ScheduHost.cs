using Task.Schedu.Utility;
using Owin_Nancy;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Task.Schedu.Utility.Mef;
using Task.Schedu.ConfigHandler;
using Task.Schedu.Quarzt;
using Task.Schedu.Utility.Reflection;
using Task.Schedu.Model;

namespace Task.Schedu.Host
{
    public partial class ScheduHost : ServiceBase
    {
        public ScheduHost()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            DebuggableAttribute att = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttribute<DebuggableAttribute>();
            if (att.IsJITTrackingEnabled)
                //Debug模式才让线程停止10s,方便附加到进程调试
                Thread.Sleep(10000);
            //配置信息读取
            ConfigInit.Init();
            //3.系统参数配置初始化
            MefConfig.Init();
            ConfigManager configManager = MefConfig.TryResolve<ConfigManager>();
            configManager.Init();
            QuartzHelper.InitScheduler();
            QuartzHelper.StartScheduler();
            // 保持web服务运行  
            ThreadPool.QueueUserWorkItem((o) =>
            {
                //启动站点
                Startup.Start(SystemConfig.WebHost, SystemConfig.WebPort);
            });
        }

        protected override void OnStop()
        {
            QuartzHelper.StopSchedule();
            //回收资源
            Startup.Dispose();
            System.Environment.Exit(0);
        }
    }
}
