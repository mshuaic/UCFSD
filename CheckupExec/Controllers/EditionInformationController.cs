using CheckupExec.Models.BEMCLIModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    public class EditionInformationController
    {
        private const string _getEditionInformationScript = Constants.GetEditionInformation + " ";
        private const string _converttoJsonString         = "| " + Constants.JsonPipeline;

        private List<EditionInformation> invokeGetEditionInformation(string scriptToInvoke)
        {
            List<EditionInformation> editionInformation = new List<EditionInformation>();

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                editionInformation = (output.Count > 0) ? JsonHelper.ConvertFromJson<EditionInformation>(output[0]) : editionInformation;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return editionInformation;
        }

        public List<EditionInformation> GetEditionInformation()
        {
            return invokeGetEditionInformation(_getEditionInformationScript);
        }
    }
}
