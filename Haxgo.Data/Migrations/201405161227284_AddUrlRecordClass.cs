namespace Haxgo.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrlRecordClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UrlRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EntityId = c.Guid(nullable: false),
                        EntityName = c.String(nullable: false, maxLength: 250),
                        Slug = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UrlRecord");
        }
    }
}
