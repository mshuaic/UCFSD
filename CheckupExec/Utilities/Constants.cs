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

        public const int SUCCESSFUL_JOB_STATUS = 9;

        //data to backup + 20% to account for growth (used in the case that a forecast cannot be made)
        public const double RECC_COVERAGE_FACTOR = 1.2;

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

        //tier discounts are applied per order -> accounting for growth earlier on
        //could reduce customer costs (i.e. better to buy 6TB than 5TB now and 1TB later [presumably])

        public static readonly Dictionary<int[], string> TierDiscounts = new Dictionary<int[], string>
        {
            [new int[] { 1 }]                                      = "1 TB",
            [new int[] { 2, 3, 4, 5 }]                             = "2 to 5 TB",
            [new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }]     = "6 to 15 TB",
            [new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 }] = "16 to 25 TB",
            [new int[] { 26 }]                                     = "26 plus TB"
        };

        //'V-ray edition' is a separate licensing model -> don't run capacity report in this case
        //'A la carte' is another licensing model -> don't run capacity report in this case

        //true -> offered by edition (per front end TB)
        //false -> not offered by edition 
        //false in capacityedition -> not offered by either edition/not listed in licensing guide

        public static readonly Dictionary<string, bool> CapacityEditionCoverage = new Dictionary<string, bool>
        {
            ["Library Expansion Option"]                    = true,
            ["SAN Shared Storage Option"]                   = false,
            ["Agent for Microsoft SharePoint"]              = true,
            ["Agent for Microsoft Exchange Server"]         = true,
            ["Agent for Lotus Domino"]                      = true,
            ["Agent for Microsoft SQL Server"]              = true,
            ["Agent for Windows"]                           = true,
            ["Copy Server Configurations"]                  = false,
            ["Central Admin Server Option"]                 = true,
            ["Agent for Linux"]                             = true,
            ["Managed Backup Exec Server"]                  = false,
            ["Advanced Disk-based Backup Option"]           = true,
            ["Agent for Oracle Servers"]                    = true,
            ["Agent for Microsoft Active Directory"]        = true,
            ["NDMP Option"]                                 = true,
            ["Agent for Mac"]                               = true,
            ["Agent for Enterprise Vault"]                  = true,
            ["Agent for Hyper-V"]                           = true,
            ["Agent for VMware Virtual Infratructure"]      = true,
            ["Remote Media Agent for Linux"]                = true,
            ["Deduplication Option"]                        = true,
            ["Virtual Tape Library Unlimited Drive Option"] = true,
            ["Agent for Applications and Databases"]        = true,
            ["Agent for VMware and Hyper-V"]                = true,
            ["Enterprise Server Option"]                    = true,
            ["V-Ray Low Density Usage"]                     = false,
            ["V-Ray High Density Usage"]                    = false
        };

        public static readonly Dictionary<string, bool> CapacityEditionLiteCoverage = new Dictionary<string, bool>
        {
            ["Library Expansion Option"]                    = false,
            ["SAN Shared Storage Option"]                   = false,
            ["Agent for Microsoft SharePoint"]              = true,
            ["Agent for Microsoft Exchange Server"]         = true,
            ["Agent for Lotus Domino"]                      = true,
            ["Agent for Microsoft SQL Server"]              = true,
            ["Agent for Windows"]                           = true,
            ["Copy Server Configurations"]                  = false,
            ["Central Admin Server Option"]                 = false,
            ["Agent for Linux"]                             = true,
            ["Managed Backup Exec Server"]                  = false,
            ["Advanced Disk-based Backup Option"]           = false,
            ["Agent for Oracle Servers"]                    = true,
            ["Agent for Microsoft Active Directory"]        = true,
            ["NDMP Option"]                                 = false,
            ["Agent for Mac"]                               = false,
            ["Agent for Enterprise Vault"]                  = true,
            ["Agent for Hyper-V"]                           = true,
            ["Agent for VMware Virtual Infratructure"]      = true,
            ["Remote Media Agent for Linux"]                = false,
            ["Deduplication Option"]                        = false,
            ["Virtual Tape Library Unlimited Drive Option"] = false,
            ["Agent for Applications and Databases"]        = true,
            ["Agent for VMware and Hyper-V"]                = true,
            ["Enterprise Server Option"]                    = false,
            ["V-Ray Low Density Usage"]                     = false,
            ["V-Ray High Density Usage"]                    = false
        };
    }
}
