using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class LicenseInformationController
    {
        private const string _getLicenseInformationScript = "get-belicenseinformation ";
        private const string _converttoJsonString = "| convertto-json";

        private List<LicenseInformation> invokeGetLicenseInformation(string scriptToInvoke)
        {
            List<LicenseInformation> licenses = null;

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                licenses = (output.Count > 0) ? JsonHelper.ConvertFromJson<LicenseInformation>(output[0]) : null;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return licenses;
        }

        public List<LicenseInformation> GetLicenses()
        {
            return invokeGetLicenseInformation(_getLicenseInformationScript);
        }

        public List<LicenseInformation> GetLicenses(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getLicenseInformationScript;

            foreach (var parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetLicenseInformation(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<LicenseInformation> GetLicenses(Dictionary<string, Dictionary<string, string>> pipelineCommands)
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

            scriptToInvoke += "| " + _getLicenseInformationScript;

            return invokeGetLicenseInformation(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<LicenseInformation> GetLicenses(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> storageParameters)
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

            scriptToInvoke += "| " + _getLicenseInformationScript;
            foreach (var parameter in storageParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetLicenseInformation(scriptToInvoke);
        }
    }
}
