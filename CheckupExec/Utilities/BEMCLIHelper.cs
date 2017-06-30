using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Security;
using System.Linq;

namespace CheckupExec.Utilities
{
    public class BEMCLIHelper
    {
        public static WSManConnectionInfo connectionInfo = null;
        public static Runspace runspace     = null;
        public static PowerShell powershell = null;

        private const int port        = 5985;
        private const string appName  = "/wsman";
        private const string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

        public BEMCLIHelper(Boolean isRemoteUser, string pass, string serverName = null, string serverUsername = null)
        {    
            //remote user and credential params fit
            if (isRemoteUser && !String.IsNullOrWhiteSpace(pass) && !String.IsNullOrWhiteSpace(serverName) && !String.IsNullOrWhiteSpace(serverUsername))
            {
                try
                {
                    //LogUtility.LogInfoFunction("Entered LoadPowerShellScript.");

                    var password = new SecureString();

                    pass.ToCharArray().ToList().ForEach(p => password.AppendChar(p));

                    var cred = new PSCredential(serverUsername, password);

                    connectionInfo = new WSManConnectionInfo(false, serverName, port, appName, shellUri, cred);

                    runspace = RunspaceFactory.CreateRunspace(connectionInfo);

                    runspace.Open();

                    powershell = PowerShell.Create();

                    powershell.Runspace = runspace;
                    powershell.AddScript(Constants.ImportBEMCLI);
                    powershell.Invoke();
                    powershell.Commands.Clear();
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

                    runspace = RunspaceFactory.CreateRunspace();

                    runspace.Open();

                    powershell = PowerShell.Create();

                    powershell.Runspace = runspace;
                    powershell.AddScript(Constants.ImportBEMCLI);
                    powershell.Invoke();
                    powershell.Commands.Clear();
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
                runspace.Close();
                powershell.Dispose();
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
