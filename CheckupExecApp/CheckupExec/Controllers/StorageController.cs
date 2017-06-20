using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class StorageController
    {
        private const string _getStorageScript = "get-bestorage ";
        private const string _converttoJsonString = "| convertto-json";

        private List<Storage> invokeGetStorages(string scriptToInvoke)
        {
            List<Storage> storages = null;

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                storages = (output.Count > 0) ? JsonHelper.ConvertFromJson<Storage>(output[0]) : null;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return storages;
        }

        public List<Storage> GetStorages()
        {
            return invokeGetStorages(_getStorageScript);
        }

        public List<Storage> GetStoragesBy(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getStorageScript;

            foreach (var parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetStorages(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<Storage> GetStoragesPipeline(Dictionary<string, Dictionary<string, string>> pipelineCommands)
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

            scriptToInvoke += "| " + _getStorageScript;

            return invokeGetStorages(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<Storage> GetStoragesByPipeline(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> storageParameters)
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

            scriptToInvoke += "| " + _getStorageScript;
            foreach (var parameter in storageParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetStorages(scriptToInvoke);
        }
    }
}
