using System.ServiceProcess;

namespace Task.Schedu.Host
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {   
            ServiceBase[] ServicesToRun = new ServiceBase[] 
            { 
                new ScheduHost() 
            };
            //运行window任务
            ServiceBase.Run(ServicesToRun);
        }
    }
}
