﻿using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;

namespace CheckupExec.Controllers
{
    public class JobHistoryController
    {
        private const string _getJobHistoryScript = Constants.GetJobHistories + " ";
        private const string _converttoJsonString = "| " + Constants.JsonPipeline;

        private List<JobHistory> invokeGetJobHistories(string scriptToInvoke)
        {
            List<JobHistory> jobHistories = new List<JobHistory>();

            BEMCLIHelper.Powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.Powershell.Invoke<string>();
                jobHistories = (output.Count > 0) ? JsonHelper.ConvertFromJson<JobHistory>(output[0]) : jobHistories;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.Powershell.Commands.Clear();

            return jobHistories;
        }

        public List<JobHistory> GetJobHistories()
        {
            return invokeGetJobHistories(_getJobHistoryScript);
        }

        public List<JobHistory> GetJobHistories(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getJobHistoryScript;

            parameters = parameters ?? new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetJobHistories(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<JobHistory> GetJobHistories(Dictionary<string, Dictionary<string, string>> pipelineCommands)
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

            scriptToInvoke += _getJobHistoryScript;

            return invokeGetJobHistories(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<JobHistory> GetJobHistories(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> jobHistoryParameters)
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

            scriptToInvoke += _getJobHistoryScript;

            jobHistoryParameters = jobHistoryParameters ?? new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> parameter in jobHistoryParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetJobHistories(scriptToInvoke);
        }
    }
}
