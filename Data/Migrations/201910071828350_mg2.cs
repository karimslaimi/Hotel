namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Revenus", "id", "dbo.Reservations");
            AddForeignKey("dbo.Revenus", "id", "dbo.Reservations", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Revenus", "id", "dbo.Reservations");
            AddForeignKey("dbo.Revenus", "id", "dbo.Reservations", "id");
        }
    }
}
