using System;


namespace ReportGen
{
    class Driver
    {
        public string Name { get; }
        public int AvailableFreeSpace { get; }
        public int TotalFreeSpace { get; }
        public int TotalSize { get; }


        // To-do: other info, such as errors, jobs info..

        public Driver(string Name,int AvailableFreeSpace, int TotalFreeSpace, int TotalSize)
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
