using System;
using System.ServiceProcess;

namespace WinServiceBase
{
    public class Program
    {
        enum RunMode { NONE, INSTALL, UNINSTALL, CONSOLE, RETHROW};

        static int Main(string[] args)
        {
            RunMode runMode = GetRunMode(args);
            try
            {
                switch (runMode)
                {
                    case RunMode.INSTALL:
                        SelfInstaller.Install(args);
                        break;
                    case RunMode.UNINSTALL:
                        SelfInstaller.Uninstall(args);
                        break;
                    case RunMode.CONSOLE:
                        RunConsoleMode();
                        break;
                    case RunMode.RETHROW:
                        RunService();
                        break;
                    default:
                        break;
                }
                return 0;
            }
            catch (Exception ex)
            {
                if (runMode == RunMode.RETHROW) throw;
                Console.WriteLine($"Error Running: {ex.Message}");
                return -1;
            }
        }

        private static RunMode GetRunMode(string[] args)
        {
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-i":
                    case "-install":
                        return RunMode.INSTALL;
                    case "-u":
                    case "-uninstall":
                        return RunMode.UNINSTALL;
                    case "-c":
                    case "-console":
                        return RunMode.CONSOLE;
                    case "-r":
                        return RunMode.RETHROW;
                    default:
                        Console.WriteLine($"Argument not expected {string.Join("|", args)}");
                        break;
                }
            }
            return RunMode.NONE;
        }

        private static void RunService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new WinService() };
            ServiceBase.Run(ServicesToRun);
        }

        private static void RunConsoleMode()
        {
            var srv = new WinService();
            Console.WriteLine("Starting ...");
            srv.StartUp();
            Console.WriteLine("System running; press any key to stop");
            Console.ReadKey(true);
            srv.ShutDown();
            Console.WriteLine("System stopped");
            Console.ReadLine();
        }
    }
}

