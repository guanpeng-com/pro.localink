namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Door_DepartId_NULL : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.localink_Doors", "DepartId", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.localink_Doors", "DepartId", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
