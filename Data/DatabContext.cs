using Hotel.Models;
using Model;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Reservation>()
              .HasOptional(x => x.revenu)
              .WithRequired(x => x.reservation)
              .WillCascadeOnDelete(true);


        }
        public DbSet<Depenses> Depenses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Revenu> Revenus { get; set; }
    }
}