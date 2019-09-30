using Hotel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Revenu
    {
        [Key, ForeignKey("reservation")]
        public int id { get; set; }
        public string devise { get; set; }
        public float montant { get; set; }

        public string type { get; set; }
       
        public DateTime daterev { get; set; }

        public virtual Reservation reservation { get; set; }
    }
}
