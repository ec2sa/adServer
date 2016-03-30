namespace ADServerDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collectionDevices : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Device_Id", "dbo.Devices");
            DropIndex("dbo.Categories", new[] { "Device_Id" });
            CreateTable(
                "dbo.DeviceCategories",
                c => new
                    {
                        Device_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Device_Id, t.Category_Id })
                .ForeignKey("dbo.Devices", t => t.Device_Id, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Device_Id)
                .Index(t => t.Category_Id);
            
            DropColumn("dbo.Categories", "Device_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Device_Id", c => c.Int());
            DropForeignKey("dbo.DeviceCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.DeviceCategories", "Device_Id", "dbo.Devices");
            DropIndex("dbo.DeviceCategories", new[] { "Category_Id" });
            DropIndex("dbo.DeviceCategories", new[] { "Device_Id" });
            DropTable("dbo.DeviceCategories");
            CreateIndex("dbo.Categories", "Device_Id");
            AddForeignKey("dbo.Categories", "Device_Id", "dbo.Devices", "Id");
        }
    }
}
