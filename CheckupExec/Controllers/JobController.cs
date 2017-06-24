﻿using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class JobController
    {
        private const string _getJobsScript = "get-bejob ";
        private const string _converttoJsonString = "| convertto-json";

        private List<Job> invokeGetJobs(string scriptToInvoke)
        {
            List<Job> jobs = null;

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                jobs = (output.Count > 0) ? JsonHelper.ConvertFromJson<Job>(output[0]) : null;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return jobs;
        }

        public List<Job> GetJobs()
        {
            return invokeGetJobs(_getJobsScript);
        }

        public List<Job> GetJobs(Dictionary<string, string> parameters)
        {
            string scriptToInvoke = _getJobsScript;

            foreach (var parameter in parameters)
            {
                scriptToInvoke += "-" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetJobs(scriptToInvoke);
        }

        //get-bealert {| get-be<..> {-k j}*}+ | convertto-json
        public List<Job> GetJobs(Dictionary<string, Dictionary<string, string>> pipelineCommands)
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

            scriptToInvoke += "| " + _getJobsScript;

            return invokeGetJobs(scriptToInvoke);
        }

        //get-bealert {-x y}+ {| get-be<> {-k j}*}+ | convertto-json
        public List<Job> GetJobs(Dictionary<string, Dictionary<string, string>> pipelineCommands, Dictionary<string, string> jobParameters)
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

            scriptToInvoke += "| " + _getJobsScript;
            foreach (var parameter in jobParameters)
            {
                scriptToInvoke += " -" + parameter.Key + " " + parameter.Value + " ";
            }

            return invokeGetJobs(scriptToInvoke);
        }
    }
}
