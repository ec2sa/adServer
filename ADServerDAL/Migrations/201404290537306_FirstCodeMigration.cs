namespace ADServerDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstCodeMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 500),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        AdPoints = c.Decimal(nullable: false, precision: 12, scale: 6),
                        ViewValue = c.Decimal(nullable: false, precision: 12, scale: 6),
                        ClickValue = c.Decimal(nullable: false, precision: 12, scale: 6),
                        PriorityId = c.Int(nullable: false),
                        UserId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Priorities", t => t.PriorityId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PriorityId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 150),
                        Device_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Device_Id)
                .Index(t => t.Device_Id);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestDate = c.DateTime(nullable: false),
                        ResponseDate = c.DateTime(nullable: false),
                        RequestIP = c.String(),
                        Data1 = c.String(),
                        Data2 = c.String(),
                        Data3 = c.String(),
                        Data4 = c.String(),
                        SessionId = c.String(),
                        RequestSource = c.Int(nullable: false),
                        Clicked = c.Boolean(nullable: false),
                        AdPoints = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MultimediaObjectId = c.Int(nullable: false),
                        DeviceId = c.Int(),
                        UserId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 150),
                        Category_Id = c.Int(),
                        Campaign_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.DeviceId)
                .ForeignKey("dbo.MultimediaObjects", t => t.MultimediaObjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id)
                .Index(t => t.MultimediaObjectId)
                .Index(t => t.DeviceId)
                .Index(t => t.UserId)
                .Index(t => t.Category_Id)
                .Index(t => t.Campaign_Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        TypeId = c.Int(nullable: false),
                        UserId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.TypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MultimediaObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 250),
                        MimeType = c.String(maxLength: 100),
                        Url = c.String(maxLength: 200),
                        Contents = c.Binary(),
                        Thumbnail = c.Binary(),
                        TypeId = c.Int(),
                        UserId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.TypeId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.TypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        AdditionalInfo = c.String(),
                        IsBlocked = c.Boolean(nullable: false),
                        AdPoints = c.Decimal(nullable: false, precision: 12, scale: 6),
                        CompanyName = c.String(),
                        CompanyAddress = c.String(),
                        Url = c.String(),
                        RoleId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleType = c.String(),
                        Commission = c.Short(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeletedDevices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Campaign_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id)
                .Index(t => t.Campaign_Id);
            
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryCampaigns",
                c => new
                    {
                        Category_Id = c.Int(nullable: false),
                        Campaign_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_Id, t.Campaign_Id })
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.Campaign_Id);
            
            CreateTable(
                "dbo.DeviceCampaigns",
                c => new
                    {
                        Device_Id = c.Int(nullable: false),
                        Campaign_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Device_Id, t.Campaign_Id })
                .ForeignKey("dbo.Devices", t => t.Device_Id, cascadeDelete: true)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id, cascadeDelete: true)
                .Index(t => t.Device_Id)
                .Index(t => t.Campaign_Id);
            
            CreateTable(
                "dbo.MultimediaObjectCampaigns",
                c => new
                    {
                        MultimediaObject_Id = c.Int(nullable: false),
                        Campaign_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MultimediaObject_Id, t.Campaign_Id })
                .ForeignKey("dbo.MultimediaObjects", t => t.MultimediaObject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Campaigns", t => t.Campaign_Id, cascadeDelete: true)
                .Index(t => t.MultimediaObject_Id)
                .Index(t => t.Campaign_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "UserId", "dbo.Users");
            DropForeignKey("dbo.Statistics", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", "PriorityId", "dbo.Priorities");
            DropForeignKey("dbo.DeletedDevices", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.Statistics", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Statistics", "UserId", "dbo.Users");
            DropForeignKey("dbo.Statistics", "MultimediaObjectId", "dbo.MultimediaObjects");
            DropForeignKey("dbo.Statistics", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "UserId", "dbo.Users");
            DropForeignKey("dbo.Devices", "TypeId", "dbo.Types");
            DropForeignKey("dbo.MultimediaObjects", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.MultimediaObjects", "TypeId", "dbo.Types");
            DropForeignKey("dbo.MultimediaObjectCampaigns", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.MultimediaObjectCampaigns", "MultimediaObject_Id", "dbo.MultimediaObjects");
            DropForeignKey("dbo.Categories", "Device_Id", "dbo.Devices");
            DropForeignKey("dbo.DeviceCampaigns", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.DeviceCampaigns", "Device_Id", "dbo.Devices");
            DropForeignKey("dbo.CategoryCampaigns", "Campaign_Id", "dbo.Campaigns");
            DropForeignKey("dbo.CategoryCampaigns", "Category_Id", "dbo.Categories");
            DropIndex("dbo.MultimediaObjectCampaigns", new[] { "Campaign_Id" });
            DropIndex("dbo.MultimediaObjectCampaigns", new[] { "MultimediaObject_Id" });
            DropIndex("dbo.DeviceCampaigns", new[] { "Campaign_Id" });
            DropIndex("dbo.DeviceCampaigns", new[] { "Device_Id" });
            DropIndex("dbo.CategoryCampaigns", new[] { "Campaign_Id" });
            DropIndex("dbo.CategoryCampaigns", new[] { "Category_Id" });
            DropIndex("dbo.DeletedDevices", new[] { "Campaign_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.MultimediaObjects", new[] { "UserId" });
            DropIndex("dbo.MultimediaObjects", new[] { "TypeId" });
            DropIndex("dbo.Devices", new[] { "UserId" });
            DropIndex("dbo.Devices", new[] { "TypeId" });
            DropIndex("dbo.Statistics", new[] { "Campaign_Id" });
            DropIndex("dbo.Statistics", new[] { "Category_Id" });
            DropIndex("dbo.Statistics", new[] { "UserId" });
            DropIndex("dbo.Statistics", new[] { "DeviceId" });
            DropIndex("dbo.Statistics", new[] { "MultimediaObjectId" });
            DropIndex("dbo.Categories", new[] { "Device_Id" });
            DropIndex("dbo.Campaigns", new[] { "UserId" });
            DropIndex("dbo.Campaigns", new[] { "PriorityId" });
            DropTable("dbo.MultimediaObjectCampaigns");
            DropTable("dbo.DeviceCampaigns");
            DropTable("dbo.CategoryCampaigns");
            DropTable("dbo.Priorities");
            DropTable("dbo.DeletedDevices");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.MultimediaObjects");
            DropTable("dbo.Types");
            DropTable("dbo.Devices");
            DropTable("dbo.Statistics");
            DropTable("dbo.Categories");
            DropTable("dbo.Campaigns");
        }
    }
}
