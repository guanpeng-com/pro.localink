namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_CommunityIdArray_To_Role : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpRoles", "CommunityIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpRoles", "CommunityIds");
        }
    }
}
