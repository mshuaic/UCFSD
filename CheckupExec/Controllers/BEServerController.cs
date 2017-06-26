﻿using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    public class BEServerController
    {
        private const string _getBEServersScript = Constants.GetBEServers + " ";
        private const string _converttoJsonString = "| " + Constants.JsonPipeline;

        private List<BEServer> invokeGetBEServers(string scriptToInvoke)
        {
            List<BEServer> beServers = new List<BEServer>();

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                beServers = (output.Count > 0) ? JsonHelper.ConvertFromJson<BEServer>(output[0]) : beServers;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return beServers;
        }

        public List<BEServer> GetBEServers()
        {
            return invokeGetBEServers(_getBEServersScript);
        }

        public List<BEServer> GetBEServers(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getBEServersScript;

            parameters = parameters ?? new Dictionary<string, string>();

            foreach (var parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " '" + parameter.Value + "' ";
            }

            return invokeGetBEServers(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<BEServer> GetBEServers(Dictionary<string, Dictionary<string, string>> pipelineCommands)
        {
            string scriptToInvoke = "";
            int numCommands = pipelineCommands.Count;

            pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();

            foreach (var pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (var parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " '" + parameter.Value + "' ";
                }

                scriptToInvoke += "| ";
            }

            scriptToInvoke += _getBEServersScript;

            return invokeGetBEServers(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<BEServer> GetBEServers(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> beServerParameters)
        {
            string scriptToInvoke = "";
            int numCommands = pipelineCommands.Count;

            pipelineCommands = pipelineCommands ?? new Dictionary<string, Dictionary<string, string>>();

            foreach (var pipeline in pipelineCommands)
            {
                scriptToInvoke += pipeline.Key + " ";

                foreach (var parameter in pipeline.Value)
                {
                    scriptToInvoke += " -" + parameter.Key + " '" + parameter.Value + "' ";
                }

                scriptToInvoke += "| ";
            }

            scriptToInvoke += _getBEServersScript;

            beServerParameters = beServerParameters ?? new Dictionary<string, string>();

            foreach (var parameter in beServerParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " '" + parameter.Value + "' ";
            }

            return invokeGetBEServers(scriptToInvoke);
        }
    }
}
