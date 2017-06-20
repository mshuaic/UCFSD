using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class AlertHistoryController
    {
        private const string _getAlertHistoriesScript = "get-beAlertHistory ";
        private const string _converttoJsonString = "| convertto-json";

        private List<Alert> invokeGetAlertHistories(string scriptToInvoke)
        {
            List<Alert> alertHistories = null;

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                alertHistories = (output.Count > 0) ? JsonHelper.ConvertFromJson<Alert>(output[0]) : null;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return alertHistories;
        }

        public List<Alert> GetAlertHistories()
        {
            return invokeGetAlertHistories(_getAlertHistoriesScript);
        }

        public List<Alert> GetAlertHistoriesBy(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getAlertHistoriesScript;

            foreach (var parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetAlertHistories(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<Alert> GetAlertHistoriesPipeline(Dictionary<string, Dictionary<string, string>> pipelineCommands)
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

            scriptToInvoke += "| " + _getAlertHistoriesScript;

            return invokeGetAlertHistories(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<Alert> GetAlertHistoriesByPipeline(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> alertHistoryParameters)
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

            scriptToInvoke += "| " + _getAlertHistoriesScript;
            foreach (var parameter in alertHistoryParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetAlertHistories(scriptToInvoke);
        }
    }
}

