namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchanging : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "methpaie", c => c.String(nullable: false));
            AddColumn("dbo.Reservations", "comfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reservations", "devise", c => c.String(nullable: false));
            AddColumn("dbo.Depenses", "pmethod", c => c.String(nullable: false));
            AddColumn("dbo.Depenses", "datedep", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Depenses", "datedep");
            DropColumn("dbo.Depenses", "pmethod");
            DropColumn("dbo.Reservations", "devise");
            DropColumn("dbo.Reservations", "comfirmed");
            DropColumn("dbo.Reservations", "methpaie");
        }
    }
}
