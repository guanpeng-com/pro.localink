namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_More_Prop_To_Delivery_Message : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_Deliverys", "CommunityName", c => c.String());
            AddColumn("dbo.localink_Messages", "CommunityName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_Messages", "CommunityName");
            DropColumn("dbo.localink_Deliverys", "CommunityName");
        }
    }
}
