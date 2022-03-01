using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WinServiceBase
{
    [RunInstaller(true)]
    public sealed class MyServiceInstallerProcess : ServiceProcessInstaller
    {
        public MyServiceInstallerProcess()
        {
            this.Account = ServiceAccount.LocalSystem;
        }
    }

    [RunInstaller(true)]
    public sealed class MyServiceInstaller : ServiceInstaller
    {
        public MyServiceInstaller()
        {
            this.Description = ConfigurationManager.AppSettings["ServiceDescription"];
            this.DisplayName = ConfigurationManager.AppSettings["ServiceDisplayName"];
            this.ServiceName = ConfigurationManager.AppSettings["ServiceName"];

            this.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.AfterInstall += ServiceInstaller_AfterInstall;
            this.BeforeUninstall += ServiceInstaller_BeforeUninstall;
        }

        void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(this.ServiceName))
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        void ServiceInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(this.ServiceName))
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                }
            }
        }
    }
}
