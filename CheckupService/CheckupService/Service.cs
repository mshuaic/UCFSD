using System;
using System.Timers;

namespace CheckupService
{
    class Service
    {
        int interval;
        Timer timer;
        logger mylogger;
        public Service(int interval=60000,string path=@"D:\CEInfo.xml")
        {
            this.interval = interval;
            this.timer = new Timer();
            this.mylogger = new logger(path);
            timer.Elapsed += new ElapsedEventHandler(mylogger._logger);
            timer.Interval = interval;
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
