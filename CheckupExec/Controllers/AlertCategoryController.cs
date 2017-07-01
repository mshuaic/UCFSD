using CheckupExec.Models.BEMCLIModels;
using CheckupExec.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Controllers
{
    public class AlertCategoryController
    {
        private const string _getAlertCategoriesScript = Constants.GetAlertCategories + " ";
        private const string _converttoJsonString = "| " + Constants.JsonPipeline;

        private List<AlertCategory> invokeGetAlertCategories(string scriptToInvoke)
        {
            List<AlertCategory> alertCategories = new List<AlertCategory>();

            BEMCLIHelper.powershell.AddScript(scriptToInvoke + _converttoJsonString);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                alertCategories = (output.Count > 0) ? JsonHelper.ConvertFromJson<AlertCategory>(output[0]) : alertCategories;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            BEMCLIHelper.powershell.Commands.Clear();

            return alertCategories;
        }

        public List<AlertCategory> GetAlertCategories()
        {
            return invokeGetAlertCategories(_getAlertCategoriesScript);
        }
    }
}
