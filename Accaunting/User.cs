using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accaunting
{
    public class User
    {
        public User()
        {
        }
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public DateTime createDate { get; set; }
        public override string ToString() { return this.username; }
    }
}
