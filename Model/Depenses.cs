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
        [Required]
        public string motif { get; set; }
        [Required]
        public float montant { get; set; }
        [Required]
        public string description { get; set; }
    }
}