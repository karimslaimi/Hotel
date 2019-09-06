using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hotel.Data
{
    public class DatabContext : DbContext
    {
        public DatabContext() : base("Name=Hotel")
        {

        }


        public DbSet<Depenses> Depenses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}