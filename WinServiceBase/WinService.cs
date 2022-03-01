using System;
using System.ServiceProcess;

namespace WinServiceBase
{
    internal class WinService : ServiceBase
    {
        #region Windows Service Lifecycle

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            StartUp();
        }

        protected override void OnStop()
        {
            ShutDown();
        }

        #endregion

        #region Start and StopMethods
        public void StartUp()
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Service Started");
        }

        public void ShutDown()
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Service Stopped");
        }
        #endregion
    }
}
