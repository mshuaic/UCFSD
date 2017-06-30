using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    public class DiskCapacity : IComparable<DiskCapacity>
    {
        public DateTime Date { get; set; }

        public long Bytes { get; set; }

        public int CompareTo(DiskCapacity dc)
        {
            if (dc.Date > this.Date)
            {
                return 1;
            }
            else if (dc.Date < this.Date)
            {
                return -1;
            }
            return 0;
        }
    }
}
