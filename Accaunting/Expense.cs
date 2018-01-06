using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accaunting
{
    public class Expense
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public double amount { get; set; }
        [Required]
        public DateTime date { get; set; }

        // FKs

        [Required]
        [ForeignKey("ExpenseCategory")]
        public int category_id { get; set; }
        public virtual ExpenseCategory ExpenseCategory { get; set; }

        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }
        public virtual User User { get; set; }

        public override string ToString() { return this.date.ToLongDateString(); }
    }
}
