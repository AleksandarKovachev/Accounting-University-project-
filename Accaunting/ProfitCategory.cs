using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public override string ToString() { return this.name; }
    }
}
