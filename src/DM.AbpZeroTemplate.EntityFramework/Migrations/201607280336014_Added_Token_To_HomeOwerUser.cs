namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Token_To_HomeOwerUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_HomeOwerUsers", "Token", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_HomeOwerUsers", "Token");
        }
    }
}
