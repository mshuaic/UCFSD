using System;

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
                case 16:
                    return "Yearly";
                case 8:
                    return "Monthly";
                case 6:
                    return "Weekly";
                case 2:
                    return "Daily";
                case 1:
                    return "Hourly";
                default:
                    return "None";
            }
        }
    }
}
