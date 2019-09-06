namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Depenses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        motif = c.String(),
                        montant = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        chambre = c.Int(nullable: false),
                        nomC = c.String(),
                        agence = c.String(),
                        Arrivee = c.DateTime(nullable: false),
                        nat = c.String(),
                        nombre = c.Int(nullable: false),
                        montant = c.Single(nullable: false),
                        bons = c.String(),
                        dft = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        mail = c.String(),
                        password = c.String(),
                        type = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Reservations");
            DropTable("dbo.Depenses");
        }
    }
}
