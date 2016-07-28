namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_HomeOwer_With_HomeOwerUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.localink_HomeOwerUsers", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_HomeOwerUsers", new[] { "HomeOwerId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.localink_HomeOwerUsers", "HomeOwerId");
            AddForeignKey("dbo.localink_HomeOwerUsers", "HomeOwerId", "dbo.localink_HomeOwers", "Id", cascadeDelete: true);
        }
    }
}
