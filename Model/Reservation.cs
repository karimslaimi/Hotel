using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class Reservation
    {
        [Key]
        public int id { get; set; }
        public int chambre { get; set; }
        public string nomC { get; set; }
        public string agence { get; set; }
        public DateTime Arrivee { get; set; }
        public string nat { get; set; }
        public int nombre { get; set; }
        public float montant { get; set; }
        public string bons { get; set; }
        public DateTime dft { get; set; }
    }
}