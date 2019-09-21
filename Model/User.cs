using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        [Index("unique",IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string mail { get; set; }
        [MinLength(8, ErrorMessage = "mot de passe doit avoir au moins 8 caractères")]
        public string password { get; set; }
        public string type { get; set; }// director or employee

    }
}