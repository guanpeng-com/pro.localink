namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_RelationShip_With_HomeOner_And_User : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.localink_HomeOwerUsers", "HomeOwerId", c => c.Long());
            DropColumn("dbo.localink_HomeOwerUsers", "IsAuth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_HomeOwerUsers", "IsAuth", c => c.Boolean(nullable: false));
            AlterColumn("dbo.localink_HomeOwerUsers", "HomeOwerId", c => c.Long(nullable: false));
        }
    }
}
