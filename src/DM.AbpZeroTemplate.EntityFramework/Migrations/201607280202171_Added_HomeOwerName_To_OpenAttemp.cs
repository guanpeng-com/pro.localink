namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_HomeOwerName_To_OpenAttemp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_OpenAttemps", "HomeOwerName", c => c.String(nullable: false));
            AddColumn("dbo.localink_OpenAttemps", "BrowserInfo", c => c.String(maxLength: 255));
            DropColumn("dbo.localink_OpenAttemps", "BowserInfo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_OpenAttemps", "BowserInfo", c => c.String(maxLength: 255));
            DropColumn("dbo.localink_OpenAttemps", "BrowserInfo");
            DropColumn("dbo.localink_OpenAttemps", "HomeOwerName");
        }
    }
}
