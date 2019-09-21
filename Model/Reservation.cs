﻿using Model;
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
        [Required]
        public int chambre { get; set; }
        [Required]
        public string agence { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public DateTime Arrivee { get; set; }
        [Required]
        public string nat { get; set; }
        [Required]
        public int nombre { get; set; }
        [Required]
        public float montant { get; set; }
        [Required]
        public string bons { get; set; }
        [Required]
        public DateTime dft { get; set; }

        public ICollection<Client> Clients { get; set; }
    }
}