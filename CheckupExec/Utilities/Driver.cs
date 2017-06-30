using System;


namespace CheckupExec.Utilities
{
    class Driver
    {
        public string Name { get; }
        public double AvailableFreeSpace { get; }
        public double TotalFreeSpace { get; }
        public double TotalSize { get; }


        // To-do: other info, such as errors, jobs info..

        public Driver(string Name,double AvailableFreeSpace, double TotalFreeSpace, double TotalSize)
        {
            this.Name = Name;
            this.AvailableFreeSpace = AvailableFreeSpace;
            this.TotalFreeSpace = TotalFreeSpace;
            this.TotalSize = TotalSize;
        }
        
        public string getFreePer(string format)
        {
             return ((float)AvailableFreeSpace / TotalSize * 100).ToString(format);
        }

        public string getUsedPer(string format)
        {
            return ((1-(float)AvailableFreeSpace / TotalSize) * 100).ToString(format);
        }

    }
}
