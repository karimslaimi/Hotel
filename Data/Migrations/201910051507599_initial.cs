namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        chambre = c.Int(nullable: false),
                        agence = c.String(nullable: false),
                        type = c.String(nullable: false),
                        Arrivee = c.DateTime(nullable: false),
                        nat = c.String(nullable: false),
                        nombre = c.Int(nullable: false),
                        montant = c.Single(nullable: false),
                        bons = c.String(nullable: false),
                        dft = c.DateTime(nullable: false),
                        methpaie = c.String(nullable: false),
                        comfirmed = c.Boolean(nullable: false),
                        devise = c.String(nullable: false),
                        montantpn = c.String(),
                        nbnuit = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Revenus",
                c => new
                    {
                        id = c.Int(nullable: false),
                        devise = c.String(),
                        montant = c.Single(nullable: false),
                        type = c.String(),
                        daterev = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Reservations", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Depenses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        motif = c.String(nullable: false),
                        montant = c.Single(nullable: false),
                        description = c.String(nullable: false),
                        pmethod = c.String(nullable: false),
                        datedep = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        mail = c.String(nullable: false),
                        password = c.String(),
                        type = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.mail, unique: true, name: "unique");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "idr", "dbo.Reservations");
            DropForeignKey("dbo.Revenus", "id", "dbo.Reservations");
            DropIndex("dbo.Users", "unique");
            DropIndex("dbo.Revenus", new[] { "id" });
            DropIndex("dbo.Clients", new[] { "idr" });
            DropTable("dbo.Users");
            DropTable("dbo.Depenses");
            DropTable("dbo.Revenus");
            DropTable("dbo.Reservations");
            DropTable("dbo.Clients");
        }
    }
}
