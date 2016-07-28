namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_HomOwerId_To_HomeOwerId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.localink_HomeOwerUsers", name: "HomOwerId", newName: "HomeOwerId");
            RenameIndex(table: "dbo.localink_HomeOwerUsers", name: "IX_HomOwerId", newName: "IX_HomeOwerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.localink_HomeOwerUsers", name: "IX_HomeOwerId", newName: "IX_HomOwerId");
            RenameColumn(table: "dbo.localink_HomeOwerUsers", name: "HomeOwerId", newName: "HomOwerId");
        }
    }
}
