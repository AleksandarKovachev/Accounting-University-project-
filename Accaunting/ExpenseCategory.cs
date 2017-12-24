using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accaunting
{
    public class ExpenseCategory
    {
        public ExpenseCategory()
        {
        }
        public int id { get; set; }
        public string name { get; set; }
        public override string ToString() { return this.name; }
    }
}
