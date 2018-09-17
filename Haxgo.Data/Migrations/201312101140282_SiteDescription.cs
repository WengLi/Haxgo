namespace Haxgo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "Description");
        }
    }
}
