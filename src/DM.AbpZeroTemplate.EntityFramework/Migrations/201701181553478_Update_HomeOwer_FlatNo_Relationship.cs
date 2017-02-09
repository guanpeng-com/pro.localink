namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_HomeOwer_FlatNo_Relationship : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.localink_HomeOwerFlatNos", name: "FlatNoId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.localink_HomeOwerFlatNos", name: "HomeOwerId", newName: "FlatNoId");
            RenameColumn(table: "dbo.localink_HomeOwerFlatNos", name: "__mig_tmp__0", newName: "HomeOwerId");
            RenameIndex(table: "dbo.localink_HomeOwerFlatNos", name: "IX_FlatNoId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.localink_HomeOwerFlatNos", name: "IX_HomeOwerId", newName: "IX_FlatNoId");
            RenameIndex(table: "dbo.localink_HomeOwerFlatNos", name: "__mig_tmp__0", newName: "IX_HomeOwerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.localink_HomeOwerFlatNos", name: "IX_HomeOwerId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.localink_HomeOwerFlatNos", name: "IX_FlatNoId", newName: "IX_HomeOwerId");
            RenameIndex(table: "dbo.localink_HomeOwerFlatNos", name: "__mig_tmp__0", newName: "IX_FlatNoId");
            RenameColumn(table: "dbo.localink_HomeOwerFlatNos", name: "HomeOwerId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.localink_HomeOwerFlatNos", name: "FlatNoId", newName: "HomeOwerId");
            RenameColumn(table: "dbo.localink_HomeOwerFlatNos", name: "__mig_tmp__0", newName: "FlatNoId");
        }
    }
}
