using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    class AlertController
    {
        private string getAlertsScript = "get-bealert | convertto-json";

        public List<Alert> GetAlerts()
        {
            List<Alert> alerts = null;

            BEMCLIHelper.powershell.AddScript(getAlertsScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                alerts = JsonHelper.ConvertFromJson<Alert>(output[0]);
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

        public List<Alert> GetAlertsBy(Dictionary<string, string> parameters)
        {
            List<Alert> alerts = null;

            string getAlertsByScript = "get-bealert";

            foreach (var parameter in parameters)
            {
                getAlertsByScript += " -" + parameter.Key + " " + parameter.Value;
            }
            getAlertsByScript += " | convertto-json";

            BEMCLIHelper.powershell.AddScript(getAlertsByScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                alerts = JsonHelper.ConvertFromJson<Alert>(output[0]);
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
    }
}
