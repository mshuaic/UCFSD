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
        public const string ImportBEMCLI = "import-module bemcli";

        public const string JsonPipeline = "convertto-json";

        public const string GetAlerts = "get-bealert ";

        public const string GetAlertHistories = "get-bealerthistory";

        public const string GetAlertCategories = "get-bealertcategory";

        public const string GetBEServers = "get-bebackupexecserver";

        public const string GetEditionInformation = "get-beeditioninformation";

        public const string GetJobs = "get-bejob";

        public const string GetJobHistories = "get-bejobhistory";

        public const string GetLicenses = "get-belicenseinformation";

        public const string GetStorages = "get-bestorage";

        public const string GetStorageDevicePools = "get-bestoragedevicepool";

        public static readonly Dictionary<string, string> JobErrorStatuses = new Dictionary<string, string>
        {
            ["Unknown"]                 = "0",
            ["Canceled"]                = "1",
            ["Completed"]               = "2",
            ["SucceededWithExceptions"] = "3",
            ["OnHold"]                  = "4",
            ["Error"]                   = "5",
            ["Missed"]                  = "6",
            ["Recovered"]               = "7",
            ["Resumed"]                 = "8",
            ["Succeeded"]               = "9",
            ["ThresholdAbort"]          = "10",
            ["Dispatched"]              = "11",
            ["DispatchFailed"]          = "12",
            ["InvalidSchedule"]         = "13",
            ["InvalidTimeWindow"]       = "14",
            ["NotInTimeWindow"]         = "15",
            ["Queued"]                  = "16",
            ["Disable"]                 = "17",
            ["Active"]                  = "18",
            ["Ready"]                   = "19",
            ["Scheduled"]               = "20",
            ["Superseded"]              = "21",
            ["ToBeScheduled"]           = "22",
            ["Linked"]                  = "23",
            ["RuleBlocked"]             = "24"
        };

    }
}
