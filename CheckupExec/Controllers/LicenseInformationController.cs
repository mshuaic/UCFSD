using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;

namespace CheckupExec.Controllers
{
    public class LicenseInformationController
    {
        private const string _getLicenseInformationScript = Constants.GetLicenses + " ";
        private const string _converttoJsonString = "| " + Constants.JsonPipeline;

        private List<LicenseInformation> invokeGetLicenseInformation(string scriptToInvoke)
        {
            List<LicenseInformation> licenses = new List<LicenseInformation>();

            BEMCLIHelper.Powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.Powershell.Invoke<string>();
                licenses = (output.Count > 0) ? JsonHelper.ConvertFromJson<LicenseInformation>(output[0]) : licenses;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.Powershell.Commands.Clear();

            return licenses;
        }

        public List<LicenseInformation> GetLicenses()
        {
            return invokeGetLicenseInformation(_getLicenseInformationScript);
        }

        //public List<LicenseInformation> GetLicenses(Dictionary<string, string> parameters)
        //{
        //    string scriptToInvoke = _getLicenseInformationScript;

        //    parameters = parameters ?? new Dictionary<string, string>();

        //    foreach (KeyValuePair<string, string> parameter in parameters)
        //    {
        //        scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
        //    }

        //    return invokeGetLicenseInformation(scriptToInvoke);
        //}

        ////get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        //public List<LicenseInformation> GetLicenses(Dictionary<string, Dictionary<string, string>> pipelineCommands)
        //{
        //    string scriptToInvoke = "";

        //    int numCommands = pipelineCommands.Count;

        //    pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();

        //    foreach (KeyValuePair<string, Dictionary<string, string>> pipeline in pipelineCommands)
        //    {
        //        scriptToInvoke += pipeline.Key + " ";

        //        foreach (KeyValuePair<string, string> parameter in pipeline.Value)
        //        {
        //            scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
        //        }

        //        scriptToInvoke += "| ";
        //    }

        //    scriptToInvoke += _getLicenseInformationScript;

        //    return invokeGetLicenseInformation(scriptToInvoke);
        //}

        ////get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        //public List<LicenseInformation> GetLicenses(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> licenseParameters)
        //{
        //    string scriptToInvoke = "";

        //    int numCommands = pipelineCommands.Count;

        //    pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();

        //    foreach (KeyValuePair<string, Dictionary<string, string>> pipeline in pipelineCommands)
        //    {
        //        scriptToInvoke += pipeline.Key + " ";

        //        foreach (KeyValuePair<string, string> parameter in pipeline.Value)
        //        {
        //            scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
        //        }

        //        scriptToInvoke += "| ";
        //    }

        //    scriptToInvoke += _getLicenseInformationScript;

        //    licenseParameters = licenseParameters ?? new Dictionary<string, string>();

        //    foreach (KeyValuePair<string, string> parameter in licenseParameters)
        //    {
        //        scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
        //    }

        //    return invokeGetLicenseInformation(scriptToInvoke);
        //}
    }
}
