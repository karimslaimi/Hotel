namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        idc = c.Int(nullable: false, identity: true),
                        nomC = c.String(),
                        idr = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idc)
                .ForeignKey("dbo.Reservations", t => t.idr, cascadeDelete: true)
                .Index(t => t.idr);
            
            AddColumn("dbo.Reservations", "type", c => c.String());
            DropColumn("dbo.Reservations", "nomC");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "nomC", c => c.String());
            DropForeignKey("dbo.Clients", "idr", "dbo.Reservations");
            DropIndex("dbo.Clients", new[] { "idr" });
            DropColumn("dbo.Reservations", "type");
            DropTable("dbo.Clients");
        }
    }
}
