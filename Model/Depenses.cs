using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class Depenses
    {
        [Key]
        public int id { get; set; }
        public string motif { get; set; }
        public float montant { get; set; }
    }
}