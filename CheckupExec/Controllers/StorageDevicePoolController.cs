using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;

namespace CheckupExec.Controllers
{
    public class StorageDevicePoolController
    {
        private const string _getStoragePoolsScript = Constants.GetStorageDevicePools + " ";
        private const string _converttoJsonString = "| " + Constants.JsonPipeline;

        private List<Storage> invokeGetStorages(string scriptToInvoke)
        {
            List<Storage> storages = new List<Storage>();

            BEMCLIHelper.Powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.Powershell.Invoke<string>();
                storages = (output.Count > 0) ? JsonHelper.ConvertFromJson<Storage>(output[0]) : storages;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.Powershell.Commands.Clear();

            return storages;
        }

        public List<Storage> GetStoragePools()
        {
            return invokeGetStorages(_getStoragePoolsScript);
        }

        //public List<Storage> GetStoragePools(Dictionary<string, string> parameters)
        //{
        //    string scriptToInvoke = _getStoragePoolsScript;

        //    parameters = parameters ?? new Dictionary<string, string>();

        //    foreach (KeyValuePair<string, string> parameter in parameters)
        //    {
        //        scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
        //    }

        //    return invokeGetStorages(scriptToInvoke);
        //}

        ////get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        //public List<Storage> GetStoragePools(Dictionary<string, Dictionary<string, string>> pipelineCommands)
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

        //    scriptToInvoke += _getStoragePoolsScript;

        //    return invokeGetStorages(scriptToInvoke);
        //}

        ////get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        //public List<Storage> GetStoragePools(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> storagePoolParameters)
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

        //    scriptToInvoke += _getStoragePoolsScript;

        //    storagePoolParameters = storagePoolParameters ?? new Dictionary<string, string>();

        //    foreach (KeyValuePair<string, string> parameter in storagePoolParameters)
        //    {
        //        scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
        //    }

        //    return invokeGetStorages(scriptToInvoke);
        //}
    }
}