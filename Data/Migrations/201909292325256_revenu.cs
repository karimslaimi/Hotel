namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Revenus", "type", c => c.String());
            AddColumn("dbo.Revenus", "daterev", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Revenus", "daterev");
            DropColumn("dbo.Revenus", "type");
        }
    }
}
