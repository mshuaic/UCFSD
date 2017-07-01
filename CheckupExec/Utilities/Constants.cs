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
        public const string ImportBEMCLI          = "import-module bemcli";

        public const string JsonPipeline          = "convertto-json";

        public const string GetAlerts             = "get-bealert ";

        public const string GetAlertHistories     = "get-bealerthistory";

        public const string GetBEServers          = "get-bebackupexecserver";

        public const string GetEditionInformation = "get-beeditioninformation";

        public const string GetJobs               = "get-bejob";

        public const string GetJobHistories       = "get-bejobhistory";

        public const string GetLicenses           = "get-belicenseinformation";

        public const string GetStorages           = "get-bestorage";

        public const string GetStorageDevicePools = "get-bestoragedevicepool";
    }
}
