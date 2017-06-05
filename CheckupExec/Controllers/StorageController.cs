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
        private string getStoragesScript = "get-bestorage | convertto-json";

        public List<Storage> GetStorages()
        {
            List<Storage> storages = null;

            BEMCLIHelper.powershell.AddScript(getStoragesScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                storages = JsonHelper.ConvertFromJson<Storage>(output[0]);
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

        public List<Storage> GetStoragesBy(Dictionary<string, string> parameters)
        {
            List<Storage> storages = null;

            string getStoragesByScript = "get-bestorage";

            foreach (var parameter in parameters)
            {
                getStoragesByScript += " -" + parameter.Key + " " + parameter.Value;
            }
            getStoragesByScript += " | convertto-json";

            BEMCLIHelper.powershell.AddScript(getStoragesByScript);

            try
            {
                var output = BEMCLIHelper.powershell.Invoke<string>();
                storages = JsonHelper.ConvertFromJson<Storage>(output[0]);
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
    }
}
