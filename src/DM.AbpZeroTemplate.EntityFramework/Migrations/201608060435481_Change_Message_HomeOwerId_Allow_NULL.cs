namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Message_HomeOwerId_Allow_NULL : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_Messages", new[] { "HomeOwerId" });
            AlterColumn("dbo.localink_Messages", "HomeOwerId", c => c.Long());
            CreateIndex("dbo.localink_Messages", "HomeOwerId");
            AddForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_Messages", new[] { "HomeOwerId" });
            AlterColumn("dbo.localink_Messages", "HomeOwerId", c => c.Long(nullable: false));
            CreateIndex("dbo.localink_Messages", "HomeOwerId");
            AddForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers", "Id", cascadeDelete: true);
        }
    }
}
