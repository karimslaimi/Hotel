﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hotel.model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HotelEntities : DbContext
    {
        public HotelEntities()
            : base("name=HotelEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Depenses> Depenses { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Revenus> Revenus { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}