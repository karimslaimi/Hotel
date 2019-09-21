namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataannotation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "agence", c => c.String(nullable: false));
            AlterColumn("dbo.Reservations", "type", c => c.String(nullable: false));
            AlterColumn("dbo.Reservations", "nat", c => c.String(nullable: false));
            AlterColumn("dbo.Reservations", "bons", c => c.String(nullable: false));
            AlterColumn("dbo.Depenses", "motif", c => c.String(nullable: false));
            AlterColumn("dbo.Depenses", "description", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "name", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "mail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", "unique");
            AlterColumn("dbo.Users", "mail", c => c.String());
            AlterColumn("dbo.Users", "name", c => c.String());
            AlterColumn("dbo.Depenses", "description", c => c.String());
            AlterColumn("dbo.Depenses", "motif", c => c.String());
            AlterColumn("dbo.Reservations", "bons", c => c.String());
            AlterColumn("dbo.Reservations", "nat", c => c.String());
            AlterColumn("dbo.Reservations", "type", c => c.String());
            AlterColumn("dbo.Reservations", "agence", c => c.String());
        }
    }
}
