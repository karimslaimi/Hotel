namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nbnuit1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "nbnuit", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.Reservations", "montantpn", c => c.Double(nullable: false));

        }

        public override void Down()
        {
        }
    }
}
