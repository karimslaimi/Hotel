using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public string type { get; set; }

    }
}