using CheckupExec.Controllers;
using CheckupExec.Models;
using CheckupExec.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec
{
    //to-do: log utility, get-* validation(?), job-data-footprint+, 
    class DataExtraction
    {
        static void Main(string[] args)
        {
            bool remoteAccess = true;
            string password = "Veritas4935";
            string serverName = "WIN-GAEP2HNEO1O";
            string userName = "Administrator";

            var Alerts = new AlertController();
            var BEServers = new BEServerController();
            var Jobs = new JobController();
            var JobHistories = new JobHistoryController();
            var Storages = new StorageController();

            new BEMCLIHelper(remoteAccess, password, serverName, userName);

            if (BEMCLIHelper.powershell != null)
            {
                var alerts = Alerts.GetAlerts();
                var beServers = BEServers.GetBEServer();
                var jobs = Jobs.GetJobs();
                var jobHistories = JobHistories.GetJobHistories();
                var storages = Storages.GetStorages();
            }
            
            //example of how you would pretty much run get-bealert -severity "warning" | convertto-json
            var parameters = new Dictionary<string, string>();
            parameters.Add("severity", "warning");

            var alertsBySeverity = Alerts.GetAlertsBy(parameters);

            foreach (var alertBySeverity in alertsBySeverity)
                Console.WriteLine(JsonHelper.JsonSerializer<Alert>(alertBySeverity));
            Console.ReadLine();


            BEMCLIHelper.cleanUp();
        }

    }
}
