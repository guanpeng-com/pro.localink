namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Relation_Community_HomeOwer_Door_Key : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_Doors", "CommunityId", c => c.Long(nullable: false));
            CreateIndex("dbo.localink_Doors", "CommunityId");
            CreateIndex("dbo.localink_HomeOwers", "CommunityId");
            AddForeignKey("dbo.localink_Doors", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_HomeOwers", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
            DropColumn("dbo.localink_Doors", "HomeOwerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_Doors", "HomeOwerId", c => c.Long(nullable: false));
            DropForeignKey("dbo.localink_HomeOwers", "CommunityId", "dbo.localink_Communities");
            DropForeignKey("dbo.localink_Doors", "CommunityId", "dbo.localink_Communities");
            DropIndex("dbo.localink_HomeOwers", new[] { "CommunityId" });
            DropIndex("dbo.localink_Doors", new[] { "CommunityId" });
            DropColumn("dbo.localink_Doors", "CommunityId");
        }
    }
}
