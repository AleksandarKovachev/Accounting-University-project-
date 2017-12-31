using System;

namespace Accaunting
{
    public class Expense
    {
        public int id { get; set; }
        public double amount { get; set; }
        public int category_id { get; set; }
        public DateTime date { get; set; }
        public int user_id { get; set; }
        public override string ToString() { return this.date.ToLongDateString(); }
    }
}
