using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    public class ServerInformation
    {
        public string Name { get; set; }

        private string si_id;
        public string Id
        {
            get
            {
                return si_id;
            }
            set
            {
                si_id = value;
            }
        }

        private string bes_id;
        public string BackupExecServerId
        {
            get
            {
                return bes_id;
            }
            set
            {
                bes_id = value;
            }
        }

        public DateTime StartDateAndTime { get; set; }

        public DateTime CurrentDataAndTime { get; set; }

        public string OperatingSystemType { get; set; }

        public string OperatingSystemMajorVersion { get; set; }

        public string OperatingSystemMinorVersion { get; set; }

        public int OperatingSystemBuild { get; set; }

        public int ProcessorType { get; set; }

        public int NumberOfProcessors { get; set; }

        public long TotalPhysicalMemoryBytes { get; set; }

        public long AvailablePhysicalMemoryBytes { get; set; }

        public long TotalVirtualMemoryBytes { get; set; }

        public long TotalPageFileSize { get; set; }
    }
}
