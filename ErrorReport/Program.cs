using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CheckupExec.Models;

namespace ErrorReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = System.IO.File.ReadAllText(@"e:\1.json");

            List<JobHistory> jobhistory = JsonConvert.DeserializeObject<List<JobHistory>>(file);

            var foo = new ErrorReportGen(@"e:\report.html",jobhistory);

            Console.ReadKey();
        }
    }
}
