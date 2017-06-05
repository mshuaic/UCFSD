using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    class JobHistory
    {
        public string Name { get; set; }

        private string jh_id;
        public string Id
        {
            get
            {
                return jh_id;
            }
            set
            {
                jh_id = value;
            }
        }

        public string JobName { get; set; }

        public string JobStatus { get; set; }
        //public string JobStatus
        //{
        //    get
        //    {
        //        return jobStatus;
        //    }
        //    set
        //    {
        //        jobStatus = value;
        //        JobStatus = jobStatus = GetJobStatusString(jobStatus);
        //    }
        //}

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ElapsedTime ElapsedTime { get; set; }

        public long TotalDataSizeBytes { get; set; }

        public double JobRateMBPerMinute { get; set; }

        public double DeduplicationRatio { get; set; }
        
        //switch?
        public int JobType { get; set; }

        public double PercentComplete { get; set; }

        public string StorageName { get; set; }

        public string BackupExecServerName { get; set; }
        
        public string JobId { get; set; }

        public string JobLogFilePath { get; set; }

        //*switches or enums for these?
        public int ErrorCode { get; set; }

        public int ErrorCategory { get; set; }
        //*

        public string ErrorMessage { get; set; }

        public string ErrorCategoryType { get; set; }

        //update later
        public static string GetJobStatusString(string id)
        {
            return "";
        }
    }
}
