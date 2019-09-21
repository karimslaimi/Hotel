namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mjadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Depenses", "description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Depenses", "description");
        }
    }
}
