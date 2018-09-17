namespace Haxgo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteShowOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "ShowOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "ShowOrder");
        }
    }
}
