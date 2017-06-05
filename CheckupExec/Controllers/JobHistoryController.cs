using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class JobHistoryController
    {
        private string getJobHistoriesScript = "get-bejobhistory | convertto-json";

        public List<JobHistory> GetJobHistories()
        {
            List<JobHistory> jobHistories = null;

            BEMCLIHelper.powershell.AddScript(getJobHistoriesScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                jobHistories = JsonHelper.ConvertFromJson<JobHistory>(output[0]);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return jobHistories;
        }

        public List<JobHistory> GetJobHistoriesBy(Dictionary<string, string> parameters)
        {
            List<JobHistory> jobHistories = null;

            string getJobHistoriesByScript = "get-bejobhistory";

            foreach (var parameter in parameters)
            {
                getJobHistoriesByScript += " -" + parameter.Key + " " + parameter.Value;
            }
            getJobHistoriesByScript += " | convertto-json";

            BEMCLIHelper.powershell.AddScript(getJobHistoriesByScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                jobHistories = JsonHelper.ConvertFromJson<JobHistory>(output[0]);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return jobHistories;
        }
    }
}
