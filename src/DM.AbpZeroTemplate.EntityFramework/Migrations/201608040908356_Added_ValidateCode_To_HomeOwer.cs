namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_ValidateCode_To_HomeOwer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_HomeOwers", "ValidateCode", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_HomeOwers", "ValidateCode");
        }
    }
}
