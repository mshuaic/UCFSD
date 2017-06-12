# UCFSD
Senior Design Veritas Data Analysis

Data Extraction:

--In DataExtraction.cs:
    -remoteAccess, password, serverName, userName are passed to BEMCLIHelper's constructor which will be passed from the
     our application's interface from user input.
    
    -Alerts, BEServers, Jobs, JobHistories, Storages are instances of their respective controllers
    
    -BEMCLIHelper constructor (initializes powershell and runspace)
    
    -if(..){..} is just calling the controller's 'get...' for whatever we're trying to get from bemcli (which is called
     from the controller and returned as a list of models corresponding to whatever we're trying to get)
      for example, var alerts = Alerts.GetAlerts() is calling GetAlerts() in Controllers/AlertController.cs which returns 
      a list of Models/Alert.cs. The 'var' can be replaced with List<Alert>
     
    -Between the comment and BEMCLIHelper.cleanUp() is just an example of passing parameters to the call in the controller.
     All you would need to do is create a dictionary with keys and values that go along with bemcli commands (-Key Value).
     //I'll add a way to pipeline before tuesday
     The foreach is just going through each Alert Model in the list and outputting it's properties as a string (which is
     deserialized in /Utilites/JsonHelper)
     
     -BEMCLIHelper.cleanUp() just disposes of its powershell and runspace members
     
--In /Utilities/BEMCLIhelper.cs:
    -WSManConnectionInfo: WS-Management connection info made up of user input and the consts which shouldn't need to be changed.
     Except for maybe the port since I'm not sure whether or not running this on a work network would affect this.
     
    -runspace holds whatever runspace we'll be using (if remote, then this is on the remote computer; otherwise, it's on the user's
    system). This is the powershell runspace that we use.
    
    -powershell is just powershell on the user's system
    
    -BEMCLIHelper(): 1) if the server is on a remote system, we create a securestring of the password that was passed, create a 
    PSCredential out of it and the server name and use that to make our connection to the remote system. Then start a runspace using
    that connection, and link it with the powershell instance.
                     2) Do the same thing except we don't need to make a connection since the beserver is on the user's system.
                     3) It's a remote server but we don't have enough info
                     
--In /Utilities/JsonHelper.cs:
    -ConvertFromJson(): generic main function for converting the json string returned from bemcli (.. | convertto-json).
    It parses the string into a jtoken to see if it's a a single object or an array of objects. The condition is just checking
    for that, but in the end this will just return a List of Models for whatever we're dealing with (Alerts, Jobs, etc)
    -JsonDeserialize: jsonstring -> our models
    -JsonSerializer: our model -> jsonstring
    
--In /Models/{..}
    -These are just key properties of the stuff we're looking for that i thought would be useful.
     As you saw, | convertto-json returns a lot of stuff that we won't need, so these are only fields i thought we'd use
     but we can easily add on to them if we want to. The property name has to be the same as whatever it's called in the jsonstring
     returned by bemcli, and of compatible type.
    -This is probably where we would deal with adding a corresponding string to the ints in the bedb (like storagetype: 4 -> "disk")
    -nested objects ({alertname: "", job: {..}}) usually an id field right after where ever they're used, so any kind of nested 
     object we might need can be gotten separately and then we can just store its id here to link it.
    
--In /Controllers/{..} //I'll probably change the structure of these soon but this is the basic idea
    -These are controllers for the models that we need to actually call for in powershell.
    -The top string is just the plain 'get all' command, though it might be a good idea to split of the '| convertto-json'
     so that we can add that right before it's called - i think this would help with adding pipelining.
     //One method for the call, others for returning the string (all, by, pipelined)
    -The actual Get{..}() methods work like this:
      1)create the string that represents the script that will be invoked by the powershell member of BEMCLIHelper
      2)invoke the script, and pass the jsonstring to output (output is just a string)
      3)pass output to ConverFromJson<x>() where x is whatever model the controller is working for, then we clear the powershell
      commands
      4)the List<x> is now just a List of models, and we return this to whoever called it (right now, in DataExtraction.cs)
      
 --/Analysis is where i'm planning to put the forecasting, backupjob estimates, and whatever else we decide to add
 --The LogUtility was just gonna go in /Utilities
