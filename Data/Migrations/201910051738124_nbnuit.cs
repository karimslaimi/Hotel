namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nbnuit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "montantpn", c => c.Single(nullable: false));
            AddColumn("dbo.Reservations", "nbnuit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "nbnuit");
            DropColumn("dbo.Reservations", "montantpn");
        }
    }
}
