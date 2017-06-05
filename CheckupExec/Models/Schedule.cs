using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    public class Schedule
    {
        public int RecurrenceType { get; set; }

        public string LocalizedScheduleString { get; set; }

        public int Every { get; set; }

        public DateTime StartDate { get; set; }
    }
}
