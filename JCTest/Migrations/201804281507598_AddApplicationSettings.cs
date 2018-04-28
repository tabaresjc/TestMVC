namespace JCTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.app_application_settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50, storeType: "nvarchar"),
                        Value = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, clustered: true, name: "IX_NAME");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.app_application_settings", "IX_NAME");
            DropTable("dbo.app_application_settings");
        }
    }
}
