namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_IsCollection_To_Keyholding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_KeyHoldings", "IsCollection", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_KeyHoldings", "IsCollection");
        }
    }
}
