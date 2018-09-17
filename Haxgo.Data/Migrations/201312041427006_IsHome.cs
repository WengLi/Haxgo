namespace Haxgo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsHome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "Is_Home", c => c.Boolean(nullable: false));
            AddColumn("dbo.Site", "Is_Bar", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "Is_Bar");
            DropColumn("dbo.Site", "Is_Home");
        }
    }
}
