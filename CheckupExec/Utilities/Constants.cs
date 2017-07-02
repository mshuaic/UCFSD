using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Utilities
{
    //constant bemcli commands held in one place
    public static class Constants
    {
        public const int BACKUP_JOB_TYPE = 1;

        public const string ImportBEMCLI          = "import-module bemcli";

        public const string JsonPipeline          = "convertto-json";

        public const string GetAlerts             = "get-bealert ";

        public const string GetAlertHistories     = "get-bealerthistory";

        public const string GetAlertCategories    = "get-bealertcategory";

        public const string GetBEServers          = "get-bebackupexecserver";

        public const string GetEditionInformation = "get-beeditioninformation";

        public const string GetJobs               = "get-bejob";

        public const string GetJobHistories       = "get-bejobhistory";

        public const string GetLicenses           = "get-belicenseinformation";

        public const string GetStorages           = "get-bestorage";

        public const string GetStorageDevicePools = "get-bestoragedevicepool";

        public static readonly Dictionary<string, string> JobErrorStatuses = new Dictionary<string, string>
        {
            ["0"]  = "Unknown",
            ["1"]  = "Canceled",
            ["2"]  = "Completed",
            ["3"]  = "SucceededWithExceptions",
            ["4"]  = "OnHold",
            ["5"]  = "Error",
            ["6"]  = "Missed",
            ["7"]  = "Recovered",
            ["8"]  = "Resumed",
            ["9"]  = "Succeeded",
            ["10"] = "ThresholdAbort",
            ["11"] = "Dispatched",
            ["12"] = "DispatchFailed",
            ["13"] = "InvalidSchedule",
            ["14"] = "InvalidTimeWindow",
            ["15"] = "NotInTimeWindow",
            ["16"] = "Queued",
            ["17"] = "Disable",
            ["18"] = "Active",
            ["19"] = "Ready",
            ["20"] = "Scheduled",
            ["21"] = "Superseded",
            ["22"] = "ToBeScheduled",
            ["23"] = "Linked",
            ["24"] = "RuleBlocked"
        };

    }
}
