using Hotel.Models;
using Newtonsoft.Json;
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
        [JsonIgnore]
        [ForeignKey("reservation")]
        public int idr { get; set; }

        [JsonIgnore]
        public virtual Reservation reservation {get;set;}

    }
}
