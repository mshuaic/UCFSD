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
        private const string _getJobHistoryScript = "get-bejobhistory ";
        private const string _converttoJsonString = "| convertto-json";

        private List<JobHistory> invokeGetJobHistories(string scriptToInvoke)
        {
            List<JobHistory> jobHistories = null;

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                jobHistories = (output.Count > 0) ? JsonHelper.ConvertFromJson<JobHistory>(output[0]) : null;
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

        public List<JobHistory> GetJobHistories()
        {
            return invokeGetJobHistories(_getJobHistoryScript);
        }

        public List<JobHistory> GetJobHistoriesBy(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getJobHistoryScript;

            foreach (var parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetJobHistories(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<JobHistory> GetJobHistoriesPipeline(Dictionary<string, Dictionary<string, string>> pipelineCommands)
        {
            string scriptToInvoke = "";
            int numCommands = pipelineCommands.Count;

            foreach (var pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (var parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
                }
            }

            scriptToInvoke += "| " + _getJobHistoryScript;

            return invokeGetJobHistories(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<JobHistory> GetJobHistoriesByPipeline(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> jobHistoryParameters)
        {
            string scriptToInvoke = "";
            int numCommands = pipelineCommands.Count;

            foreach (var pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (var parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
                }
            }

            scriptToInvoke += "| " + _getJobHistoryScript;
            foreach (var parameter in jobHistoryParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetJobHistories(scriptToInvoke);
        }
    }
}
