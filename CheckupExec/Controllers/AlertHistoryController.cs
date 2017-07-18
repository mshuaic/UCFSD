using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;

namespace CheckupExec.Controllers
{
    public class AlertHistoryController
    {
        private const string _getAlertHistoriesScript = Constants.GetAlertHistories + " ";
        private const string _converttoJsonString = "| " + Constants.JsonPipeline;

        private List<Alert> invokeGetAlertHistories(string scriptToInvoke)
        {
            List<Alert> alertHistories = new List<Alert>();

            BEMCLIHelper.Powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.Powershell.Invoke<string>();
                alertHistories = (output.Count > 0) ? JsonHelper.ConvertFromJson<Alert>(output[0]) : alertHistories;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.Powershell.Commands.Clear();

            return alertHistories;
        }

        public List<Alert> GetAlertHistories()
        {
            return invokeGetAlertHistories(_getAlertHistoriesScript);
        }

        public List<Alert> GetAlertHistories(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getAlertHistoriesScript;

            parameters = parameters ?? new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetAlertHistories(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<Alert> GetAlertHistories(Dictionary<string, Dictionary<string, string>> pipelineCommands)
        {
            string scriptToInvoke = "";

            int numCommands = pipelineCommands.Count;

            pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();

            foreach (KeyValuePair<string, Dictionary<string, string>> pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (KeyValuePair<string, string> parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
                }

                scriptToInvoke += "| ";
            }

            scriptToInvoke += _getAlertHistoriesScript;

            return invokeGetAlertHistories(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<Alert> GetAlertHistories(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> alertHistoryParameters)
        {
            string scriptToInvoke = "";

            int numCommands = pipelineCommands.Count;

            pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();

            foreach (KeyValuePair<string, Dictionary<string, string>> pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (var parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
                }

                scriptToInvoke += "| ";
            }

            scriptToInvoke += _getAlertHistoriesScript;

            alertHistoryParameters = alertHistoryParameters ?? new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> parameter in alertHistoryParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetAlertHistories(scriptToInvoke);
        }
    }
}

