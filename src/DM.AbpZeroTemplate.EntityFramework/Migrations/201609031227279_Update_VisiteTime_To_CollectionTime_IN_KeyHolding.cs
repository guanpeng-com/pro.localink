namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_VisiteTime_To_CollectionTime_IN_KeyHolding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_KeyHoldings", "CollectionTime", c => c.DateTime());
            DropColumn("dbo.localink_KeyHoldings", "VisiteTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_KeyHoldings", "VisiteTime", c => c.DateTime());
            DropColumn("dbo.localink_KeyHoldings", "CollectionTime");
        }
    }
}
