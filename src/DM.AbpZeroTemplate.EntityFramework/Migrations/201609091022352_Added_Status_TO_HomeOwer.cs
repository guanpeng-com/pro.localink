namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Status_TO_HomeOwer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_HomeOwers", "Status", c => c.Byte(nullable: false));
            AddColumn("dbo.localink_HomeOwerUsers", "IsAuth", c => c.Boolean(nullable: false));
            AddColumn("dbo.localink_HomeOwerUsers", "CommunityId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_HomeOwerUsers", "CommunityId");
            DropColumn("dbo.localink_HomeOwerUsers", "IsAuth");
            DropColumn("dbo.localink_HomeOwers", "Status");
        }
    }
}
