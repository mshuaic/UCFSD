using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    public class AlertController
    {
        private const string _getAlertsScript = Constants.GetAlerts + " ";
        private const string _converttoJsonString = "| " + Constants.JsonPipeline;

        private List<Alert> invokeGetAlerts(string scriptToInvoke)
        {
            List<Alert> alerts = new List<Alert>();

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                alerts = (output.Count > 0) ? JsonHelper.ConvertFromJson<Alert>(output[0]) : alerts;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return alerts;
        }

        public List<Alert> GetAlerts()
        {
            return invokeGetAlerts(_getAlertsScript);
        }

        public List<Alert> GetAlerts(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getAlertsScript;

            parameters = parameters ?? new Dictionary<string, string>();
            
            foreach (var parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetAlerts(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<Alert> GetAlerts(Dictionary<string, Dictionary<string, string>> pipelineCommands)
        {
            string scriptToInvoke = "";
            int numCommands = pipelineCommands.Count;

            pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();

            foreach (var pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (var parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
                }
            }
            
            scriptToInvoke += "| " + _getAlertsScript;

            return invokeGetAlerts(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<Alert> GetAlerts(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> alertParameters)
        {
            string scriptToInvoke = ""; 
            int numCommands = pipelineCommands.Count;

            pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();
            
            foreach (var pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (var parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
                }
            }

            scriptToInvoke += "| " + _getAlertsScript;

            alertParameters = alertParameters ?? new Dictionary<string, string>();

            foreach (var parameter in alertParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }
            
            return invokeGetAlerts(scriptToInvoke);
        }
    }
}
