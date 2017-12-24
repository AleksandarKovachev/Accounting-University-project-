using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accaunting
{
    public class ProfitCategory
    {
        public ProfitCategory()
        {
        }
        public int id { get; set; }
        public string name { get; set; }
        public override string ToString() { return this.name; }
    }
}
