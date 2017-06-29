using System;
using System.IO;
using System.Timers;
using Microsoft.Win32;

namespace CheckupService
{
    class Service
    {
        int interval;
        Timer timer;
        logger mylogger;
        string path;
        int max;

        public Service(int interval=60,string path=@"C:\CEInfo.xml",int max=50)
        {
            this.interval = interval*1000;
            this.timer = new Timer();
            this.path = path;
            this.max = max;
            this.mylogger = new logger(this.path, this.max);
            timer.Elapsed += new ElapsedEventHandler(mylogger._logger);
            timer.Interval = this.interval;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

    }
}
