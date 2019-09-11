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
    public class Client
    {
        [Key]
        public int idc { get; set; }
        public string nomC { get; set; }

        [ForeignKey("reservation")]
        public int idr { get; set; }
       

        public virtual Reservation reservation {get;set;}

    }
}
