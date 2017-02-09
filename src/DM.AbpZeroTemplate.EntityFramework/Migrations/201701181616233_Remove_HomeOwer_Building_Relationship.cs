namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_HomeOwer_Building_Relationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.localink_Buildings", "HomeOwer_Id", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_Buildings", new[] { "HomeOwer_Id" });
            DropColumn("dbo.localink_Buildings", "HomeOwer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_Buildings", "HomeOwer_Id", c => c.Long());
            CreateIndex("dbo.localink_Buildings", "HomeOwer_Id");
            AddForeignKey("dbo.localink_Buildings", "HomeOwer_Id", "dbo.localink_HomeOwers", "Id");
        }
    }
}
