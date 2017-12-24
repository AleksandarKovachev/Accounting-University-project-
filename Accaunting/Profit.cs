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
        public ProfitCategory category { get; set; }
        public override string ToString() { return this.category.name; }
    }
}
