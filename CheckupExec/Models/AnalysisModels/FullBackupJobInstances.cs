using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models.AnalysisModels
{
    public class FullBackupJobInstance
    {
        public Storage Storage { get; set; }

       public List<JobHistory> JobHistories {get; set;}
    }
}
