using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class JobController
    {
        private string getJobsScript = "get-bejob | convertto-json";

        public List<Job> GetJobs()
        {
            List<Job> jobs = null;

            BEMCLIHelper.powershell.AddScript(getJobsScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                jobs = JsonHelper.ConvertFromJson<Job>(output[0]);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return jobs;
        }

        public List<Job> GetJobsBy(Dictionary<string, string> parameters)
        {
            List<Job> jobs = null;

            string getJobsByScript = "get-bejob";

            foreach (var parameter in parameters)
            {
                getJobsByScript += " -" + parameter.Key + " " + parameter.Value;
            }
            getJobsByScript += " | convertto-json";

            BEMCLIHelper.powershell.AddScript(getJobsByScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                jobs = JsonHelper.ConvertFromJson<Job>(output[0]);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return jobs;
        }
    }
}
