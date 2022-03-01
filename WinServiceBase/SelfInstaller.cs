using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;

namespace WinServiceBase
{
    public static class SelfInstaller
    {
        public static void Install(string[] args)
        {
            DoInstallerAction(true, args);
        }

        public static void Uninstall(string[] args)
        {
            DoInstallerAction(false, args);
        }

        private static void DoInstallerAction(bool install, string[] args)
        {
            IDictionary state = new Hashtable();
            try
            {
                using (var inst = new AssemblyInstaller(Assembly.GetExecutingAssembly(), args))
                {
                    try
                    {
                        inst.UseNewContext = true;
                        if (install)
                        {
                            inst.Install(state);
                            inst.Commit(state);
                        }
                        else
                        {
                            inst.Uninstall(state);
                        }
                    }
                    catch 
                    {
                        try
                        {
                            inst.Rollback(state);
                        }
                        catch { }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Installing: {ex.Message}");
            }
        }
    }
}
