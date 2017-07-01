using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    public class UsedCapacity : IComparable<UsedCapacity>
    {
        public DateTime Date { get; set; }

        public long Bytes { get; set; }

        public int CompareTo(UsedCapacity dc)
        {
            if (dc.Date > Date)
            {
                return -1;
            }
            else if (dc.Date < Date)
            {
                return 1;
            }
            return 0;
        }
    }
}
