namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nbn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "nbnuit", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservations", "nbnuit", c => c.String());
        }
    }
}
