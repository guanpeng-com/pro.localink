namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_CommunityName_To_Building : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_HomeOwers", "CommunityName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_HomeOwers", "CommunityName");
        }
    }
}
