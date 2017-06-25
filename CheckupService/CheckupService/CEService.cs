using System;
using Topshelf;

namespace CheckupService
{
    class CEService
    {
        static void Main(string[] args)
        {
            int interval = 60000;
            string path = @"D:\CEinfo.xml";
            HostFactory.Run(x =>
            {
                x.AddCommandLineDefinition("i", i => { interval = Int32.Parse(i); });
                x.AddCommandLineDefinition("p", p => { path = p; });
                x.ApplyCommandLine();

                x.Service<Service>(s =>
                {
                    s.ConstructUsing(name => new Service(interval,path));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Disk Storage Info for Checkup Exec");
                x.SetDisplayName("Checkup Exec Service");
                x.SetServiceName("CEService");
            });

        }

    }
}
