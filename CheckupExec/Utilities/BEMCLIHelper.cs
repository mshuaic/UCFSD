using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;

namespace CheckupExec.Utilities
{
    public class BEMCLIHelper
    {
        public static WSManConnectionInfo ConnectionInfo = null;
        public static Runspace Runspace = null;
        public static PowerShell Powershell = null;

        private const int port = 5985;
        private const string appName = "/wsman";
        private const string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

        public BEMCLIHelper(bool isRemoteUser, string pass, string serverName = null, string serverUsername = null)
        {
            //remote user and credential params fit
            if (isRemoteUser && !string.IsNullOrWhiteSpace(pass) && !string.IsNullOrWhiteSpace(serverName) && !string.IsNullOrWhiteSpace(serverUsername))
            {
                try
                {
                    //LogUtility.LogInfoFunction("Entered LoadPowerShellScript.");

                    var password = new SecureString();

                    pass.ToCharArray().ToList().ForEach(p => password.AppendChar(p));

                    var cred = new PSCredential(serverUsername, password);

                    ConnectionInfo = new WSManConnectionInfo(false, serverName, port, appName, shellUri, cred);

                    Runspace = RunspaceFactory.CreateRunspace(ConnectionInfo);

                    Runspace.Open();

                    Powershell = PowerShell.Create();

                    Powershell.Runspace = Runspace;
                    Powershell.AddScript(Constants.ImportBEMCLI);
                    Powershell.Invoke();
                    Powershell.Commands.Clear();
                }
                catch (Exception e)
                {
                    Exception baseException = e.GetBaseException();
                    //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                    Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                }
            }
            //not remote user (params are not needed)
            else if (!isRemoteUser)
            {
                try
                {
                    //LogUtility.LogInfoFunction("Entered LoadPowerShellScript.");

                    Runspace = RunspaceFactory.CreateRunspace();

                    Runspace.Open();

                    Powershell = PowerShell.Create();

                    Powershell.Runspace = Runspace;
                    Powershell.AddScript(Constants.ImportBEMCLI);
                    Powershell.Invoke();
                    Powershell.Commands.Clear();
                }
                catch (Exception e)
                {
                    Exception baseException = e.GetBaseException();
                    //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                    Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                }
            }
            //remote user missing one or more params
            else
            {
                //LogUtility.LogInfoFunction("A password, server name, and username must be provided if accessing a Backup Exec Server remotely.");
                Console.WriteLine("A password, server name, and username must be provided if accessing a Backup Exec Server remotely.");
            }
        }

        //dispose
        public static bool CleanUp()
        {
            try
            {
                Runspace.Close();
                Powershell.Dispose();
                return true;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                //LogUtility.LogInfoFunction("Error:" + e.Message + "Message:" + baseException.Message);
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                return false;
            }
        }
    }
}
