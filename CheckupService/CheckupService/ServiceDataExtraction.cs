using System.Collections.Generic;
using CheckupExec;
using CheckupExec.Controllers;
using CheckupExec.Models;

namespace CheckupService
{
    public class ServiceDataExtraction : DataExtraction
    {

        public ServiceDataExtraction(bool isRemoteUser, string password = null, string serverName = null, string userName = null) 
            : base(isRemoteUser, password, serverName, userName)
        {}

        public List<Storage> GetStorage()
        {
            return StorageController.GetStorages();
        }
    }
}
