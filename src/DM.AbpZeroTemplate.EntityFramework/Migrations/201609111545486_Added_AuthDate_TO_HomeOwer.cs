namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_AuthDate_TO_HomeOwer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_HomeOwers", "AuthTime", c => c.DateTime());
            AddColumn("dbo.localink_HomeOwers", "AuthAdmin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_HomeOwers", "AuthAdmin");
            DropColumn("dbo.localink_HomeOwers", "AuthTime");
        }
    }
}
