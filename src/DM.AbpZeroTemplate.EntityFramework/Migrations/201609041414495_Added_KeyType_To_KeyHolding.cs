namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_KeyType_To_KeyHolding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_KeyHoldings", "KeyType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_KeyHoldings", "KeyType");
        }
    }
}
