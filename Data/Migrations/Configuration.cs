namespace Data.Migrations
{
    using Hotel.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Hotel.Data.DatabContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(Hotel.Data.DatabContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            string password = "admin";

            SHA256 hash = new SHA256CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(password);
            Byte[] encodedBytes = hash.ComputeHash(originalBytes);
            password = BitConverter.ToString(encodedBytes);

            var Users = new List<User>
                {


            new User{mail="karim@karim.com",name="karim",password=password,type="director"},
            new User{mail="sami@sami.com",name="sami",password=password,type="employee"}

                };
            Users.ForEach(e => context.Users.AddOrUpdate(c => c.mail, e));
            context.SaveChanges();

        }
    }
}
