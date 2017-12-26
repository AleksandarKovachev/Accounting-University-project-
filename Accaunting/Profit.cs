using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accaunting
{
    public class Profit
    {
        public Profit()
        {
        }
        public int id { get; set; }
        public double amount { get; set; }
        public int category_id { get; set; }
        public DateTime date { get; set; }
        public override string ToString() { return this.date.ToLongDateString(); }
    }
}
