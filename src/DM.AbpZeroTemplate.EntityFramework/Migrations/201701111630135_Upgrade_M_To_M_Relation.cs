namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Upgrade_M_To_M_Relation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.localink_HomeOwerDoors", name: "HomeOwerId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.localink_HomeOwerDoors", name: "DoorId", newName: "HomeOwerId");
            RenameColumn(table: "dbo.localink_HomeOwerBuildings", name: "HomeOwerId", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.localink_HomeOwerBuildings", name: "BuildingId", newName: "HomeOwerId");
            RenameColumn(table: "dbo.localink_HomeOwerDoors", name: "__mig_tmp__0", newName: "DoorId");
            RenameColumn(table: "dbo.localink_HomeOwerBuildings", name: "__mig_tmp__1", newName: "BuildingId");
            RenameIndex(table: "dbo.localink_HomeOwerDoors", name: "IX_HomeOwerId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.localink_HomeOwerDoors", name: "IX_DoorId", newName: "IX_HomeOwerId");
            RenameIndex(table: "dbo.localink_HomeOwerBuildings", name: "IX_HomeOwerId", newName: "__mig_tmp__1");
            RenameIndex(table: "dbo.localink_HomeOwerBuildings", name: "IX_BuildingId", newName: "IX_HomeOwerId");
            RenameIndex(table: "dbo.localink_HomeOwerDoors", name: "__mig_tmp__0", newName: "IX_DoorId");
            RenameIndex(table: "dbo.localink_HomeOwerBuildings", name: "__mig_tmp__1", newName: "IX_BuildingId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.localink_HomeOwerBuildings", name: "IX_BuildingId", newName: "__mig_tmp__1");
            RenameIndex(table: "dbo.localink_HomeOwerDoors", name: "IX_DoorId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.localink_HomeOwerBuildings", name: "IX_HomeOwerId", newName: "IX_BuildingId");
            RenameIndex(table: "dbo.localink_HomeOwerBuildings", name: "__mig_tmp__1", newName: "IX_HomeOwerId");
            RenameIndex(table: "dbo.localink_HomeOwerDoors", name: "IX_HomeOwerId", newName: "IX_DoorId");
            RenameIndex(table: "dbo.localink_HomeOwerDoors", name: "__mig_tmp__0", newName: "IX_HomeOwerId");
            RenameColumn(table: "dbo.localink_HomeOwerBuildings", name: "BuildingId", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.localink_HomeOwerDoors", name: "DoorId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.localink_HomeOwerBuildings", name: "HomeOwerId", newName: "BuildingId");
            RenameColumn(table: "dbo.localink_HomeOwerBuildings", name: "__mig_tmp__1", newName: "HomeOwerId");
            RenameColumn(table: "dbo.localink_HomeOwerDoors", name: "HomeOwerId", newName: "DoorId");
            RenameColumn(table: "dbo.localink_HomeOwerDoors", name: "__mig_tmp__0", newName: "HomeOwerId");
        }
    }
}
