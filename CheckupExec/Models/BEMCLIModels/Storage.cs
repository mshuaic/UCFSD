using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{   
    //we are not storing member devices [for device pools] because we are not looing directly at any device pools

    public class Storage
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string StorageType { get; set; }
        //public string StorageType
        //{
        //    get
        //    {
        //        return StorageType;
        //    }
        //    set
        //    {
        //        StorageType = value;
        //        StorageType = GetStorageTypeString(StorageType);
        //    }
        //}

        public long MaximumFileSizeBytes { get; set; }

        public Boolean PreallocationEnabled { get; set; }

        public long PreallocationIncrementBytes { get; set; }

        public long BlockSizeBytes { get; set; }

        public long BufferSizeBytes { get; set; }

        public long DiskSpaceReserveBytes { get; set; }

        public double CompressionRatio { get; set; }

        public string DaysOfStorageRemaining { get; set; }

        public int DaysOfStorageRemaingingStatus { get; set; }

        public string CapacityStatus { get; set; }
        //public string CapacityStatus
        //{
        //    get
        //    {
        //        return CapacityStatus;
        //    }
        //    set
        //    {
        //        CapacityStatus = value;
        //        CapacityStatus = GetCapacityStatusString(CapacityStatus);
        //    }
        //}

        public long TotalCapacityBytes { get; set; }

        public long TotalBackupStorageBytes { get; set; }

        public long UsedCapacityBytes { get; set; }

        public long DataWrittenBytes { get; set; }

        public long AvailableCapacityBytes { get; set; }

        //Perhaps create props for the 'isValid*' attributes?

        public DateTime DateInService { get; set; }

        public string Description { get; set; }

        public string UsesDataLifecycleManagement { get; set; }

        //Update later, maybe
        public static string GetStorageTypeString(string id)
        {
            return "";
        }

        //update later, maybe
        public static string GetCapacityStatusString(string id)
        {
            return "";
        }
    }
}
