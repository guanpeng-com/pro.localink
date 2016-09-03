namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Password_To_KeyHolding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_KeyHoldings", "Password", c => c.String(maxLength: 255));
            AddColumn("dbo.localink_KeyHoldings", "VisiteStartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.localink_KeyHoldings", "VisiteEndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.localink_KeyHoldings", "VisiteTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.localink_KeyHoldings", "VisiteTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.localink_KeyHoldings", "VisiteEndTime");
            DropColumn("dbo.localink_KeyHoldings", "VisiteStartTime");
            DropColumn("dbo.localink_KeyHoldings", "Password");
        }
    }
}
