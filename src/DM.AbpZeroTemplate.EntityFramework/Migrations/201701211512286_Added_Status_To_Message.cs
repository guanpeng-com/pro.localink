namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Status_To_Message : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_Messages", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_Messages", "Status");
        }
    }
}
