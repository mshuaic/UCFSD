using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CheckupExec.Utilities
{
    class JsonHelper
    {
        public static List<T> ConvertFromJson<T>(string jsonString)
        {
            //LogUtility.LogInfoFunction("Entered ConvertFromJson.");            
            var token = JToken.Parse(jsonString);
            if (token is JArray)
            {
                //LogUtility.LogInfoFunction("Calling JsonHelper function JsonDeserialize<List<Alert>>(JsonString); ");
                var objects = JsonDeserialize<List<T>>(jsonString);
                //LogUtility.LogInfoFunctionFinished();
                return objects;
            }
            else
            {
                //LogUtility.LogInfoFunction("Calling JsonHelper function JsonDeserialize<List<Alert>>(JsonString); ");
                var Objects = JsonDeserialize<T>(jsonString);
                List<T> result = new List<T> { Objects };
                //LogUtility.LogInfoFunctionFinished();
                return result;
            }
        }

        //string -> model
        public static T JsonDeserialize<T>(string jsonString)
        {
            //LogUtility.LogInfoFunction("Entered JsonDeserialize.");
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        //string <- model
        public static string JsonSerializer<T>(T dataObject)
        {
            //LogUtility.LogInfoFunction("Entered JsonSerializer.");
            return JsonConvert.SerializeObject(dataObject, Formatting.Indented);
        }
    }
}
