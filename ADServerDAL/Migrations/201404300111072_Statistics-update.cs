namespace ADServerDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statisticsupdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Statistics", name: "Campaign_Id", newName: "CampaignId");
            RenameIndex(table: "dbo.Statistics", name: "IX_Campaign_Id", newName: "IX_CampaignId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Statistics", name: "IX_CampaignId", newName: "IX_Campaign_Id");
            RenameColumn(table: "dbo.Statistics", name: "CampaignId", newName: "Campaign_Id");
        }
    }
}
