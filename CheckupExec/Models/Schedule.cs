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

        //correct this later!!
        public string getRecurrenceTypeString(int recurrenceType)
        {
            switch (recurrenceType)
            {
                case 1:
                    return "Yearly";
                case 2:
                    return "Monthly";
                case 6:
                    return "Weekly";
                case 4:
                    return "Daily";
                default:
                    return "Hourly";
            }
        }
    }
}
