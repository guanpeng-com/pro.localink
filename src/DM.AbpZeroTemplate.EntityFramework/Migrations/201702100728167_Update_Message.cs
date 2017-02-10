namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Message : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.localink_Messages", "IsRead", c => c.Boolean());
            AlterColumn("dbo.localink_Messages", "FlatNoId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.localink_Messages", "FlatNoId", c => c.Long(nullable: false));
            AlterColumn("dbo.localink_Messages", "IsRead", c => c.Boolean(nullable: false));
        }
    }
}
