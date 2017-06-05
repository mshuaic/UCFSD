using CheckupExec.Models;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class BEServerController
    {
        private string getBEServersScript = "get-bebackupexecserver | convertto-json";

        public List<BEServer> GetBEServer()
        {
            List<BEServer> BEServers = null;

            BEMCLIHelper.powershell.AddScript(getBEServersScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                BEServers = JsonHelper.ConvertFromJson<BEServer>(output[0]);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return BEServers;
        }

        public List<BEServer> GetBEServersBy(Dictionary<string, string> parameters)
        {
            List<BEServer> BEServers = null;

            string getBEServersByScript = "get-bebackupexecserver";

            foreach (var parameter in parameters)
            {
                getBEServersByScript += " -" + parameter.Key + " " + parameter.Value;
            }
            getBEServersByScript += " | convertto-json";

            BEMCLIHelper.powershell.AddScript(getBEServersByScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                BEServers = JsonHelper.ConvertFromJson<BEServer>(output[0]);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return BEServers;
        }
    }
}
