using System;
using System.IO;
using System.Text;
using Topshelf;
using Topshelf.Runtime;
using Microsoft.Win32;

namespace CheckupService
{

    class CEService
    {
        static string pwd = System.Environment.CurrentDirectory;
        static bool _config = false;
        static int interval = 10;
        static string path = Path.Combine(pwd, "DiskInfo.xml");
        static int max = 50;
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                // Command line options
                x.AddCommandLineSwitch("c", c => { _config = c; });
                x.AddCommandLineDefinition("i", i => { interval = Int32.Parse(i); });
                x.AddCommandLineDefinition("p", p => { path = p; });
                x.AddCommandLineDefinition("m", m => { max = Int32.Parse(m); });
                x.ApplyCommandLine();

                x.AfterInstall(
                installSettings =>
                {
                    AddCommandLineParametersToStartupOptions(installSettings);
                });

                x.Service<Service>(s =>
                {
                    s.ConstructUsing(() => new Service(interval, path, max));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.EnableServiceRecovery(s =>
                {
                    s.RestartService(1);
                    s.OnCrashOnly();
                });

                x.RunAsLocalSystem();
                x.SetDescription("Disk Storage Info for Checkup Exec");
                x.SetDisplayName("Checkup Exec Service");
                x.SetServiceName("CEService");
                x.StartAutomatically();

            });
        }

        private static void AddCommandLineParametersToStartupOptions(InstallHostSettings installSettings)
        {
            var serviceKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                $"SYSTEM\\CurrentControlSet\\Services\\{installSettings.ServiceName}",
                true);

            if (serviceKey == null)
            {
                throw new Exception($"Could not locate Registry Key for service '{installSettings.ServiceName}'");
            }

            var arguments = Environment.GetCommandLineArgs();

            string programName = null;
            StringBuilder argumentsList = new StringBuilder();

            for (int i = 0; i < arguments.Length; i++)
            {
                if (i == 0)
                {
                    // program name is the first argument
                    programName = Path.GetFileName(arguments[i]);
                }
                else
                {
                    // Remove these servicename and instance arguments as TopShelf adds them as well
                    // Remove install switch
                    if (arguments[i].StartsWith("-servicename", StringComparison.InvariantCultureIgnoreCase) |
                        arguments[i].StartsWith("-instance", StringComparison.InvariantCultureIgnoreCase) |
                        arguments[i].StartsWith("install", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    if (arguments[i].StartsWith("-p"))
                    {
                        arguments[i+1] = string.Format("\"{0}\"", arguments[i+1]);
                    }
                    argumentsList.Append(" ");
                    argumentsList.Append(arguments[i]);
                }
            }

            // Apply the arguments to the ImagePath value under the service Registry key
            var imageName = $"\"{Environment.CurrentDirectory}\\{programName}\" {argumentsList.ToString()}";
            serviceKey.SetValue("ImagePath", imageName, Microsoft.Win32.RegistryValueKind.String);
        }

    }
}
