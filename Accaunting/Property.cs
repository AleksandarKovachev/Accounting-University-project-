using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accaunting
{
    public class Property
    {
        public Property()
        {
        }
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string key { get; set; }
        public string value { get; set; }
        public override string ToString() { return this.value; }
    }
}
