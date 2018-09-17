namespace Haxgo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KeyWord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "KeyWord", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "KeyWord");
        }
    }
}
