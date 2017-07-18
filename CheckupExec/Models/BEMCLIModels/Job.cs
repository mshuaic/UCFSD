using System;

namespace CheckupExec.Models
{
    public class Job
    {
        public string Name { get; set; }

        private string job_id;
        public string Id
        {
            get
            {
                return job_id;
            }
            set
            {
                job_id = value;
            }
        }

        public string TaskName { get; set; }

        //*switch/enums?
        public int Status { get; set; }

        public int SubStatus { get; set; }

        public int JobType { get; set; }

        public int TaskType { get; set; }
        //*

        public Schedule Schedule { get; set; }

        public string SelectionSummary { get; set; }

        public DateTime NextStartDate { get; set; }

        public int Priority { get; set; }

        public string StorageId { get; set; }

        public KeepDiskDataFor KeepDiskDataFor { get; set; }

        public string MediaSetId { get; set; }

        public string MediaVaultId { get; set; }

        public Boolean IsBackupDefinitionJob { get; set; }

        //BackupDefinitionId if needed

        public string BackupExecServerId { get; set; }
    }
}
