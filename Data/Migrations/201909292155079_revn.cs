namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Revenus",
                c => new
                    {
                        id = c.Int(nullable: false),
                        devise = c.String(),
                        montant = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Reservations", t => t.id)
                .Index(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Revenus", "id", "dbo.Reservations");
            DropIndex("dbo.Revenus", new[] { "id" });
            DropTable("dbo.Revenus");
        }
    }
}
