namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_Model_Relations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.localink_HomeOwers", "CommunityId", "dbo.localink_Communities");
            DropForeignKey("dbo.localink_AccessKeys", "CommunityId", "dbo.localink_Communities");
            DropForeignKey("dbo.localink_OpenAttemps", "CommunityId", "dbo.localink_Communities");
            DropForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_AccessKeys", new[] { "CommunityId" });
            DropIndex("dbo.localink_HomeOwers", new[] { "CommunityId" });
            DropIndex("dbo.localink_Messages", new[] { "HomeOwerId" });
            DropIndex("dbo.localink_OpenAttemps", new[] { "CommunityId" });
            DropPrimaryKey("dbo.localink_HomeOwerDoors");
            CreateTable(
                "dbo.localink_Buildings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CommunityId = c.Long(nullable: false),
                        TenantId = c.Int(),
                        BuildingName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Building_AdminCommunityFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Building_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Building_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_Communities", t => t.CommunityId, cascadeDelete: true)
                .Index(t => t.CommunityId);
            
            CreateTable(
                "dbo.localink_HomeOwerBuildings",
                c => new
                    {
                        HomeOwerId = c.Long(nullable: false),
                        BuildingId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.HomeOwerId, t.BuildingId })
                .ForeignKey("dbo.localink_Buildings", t => t.HomeOwerId, cascadeDelete: true)
                .ForeignKey("dbo.localink_HomeOwers", t => t.BuildingId, cascadeDelete: true)
                .Index(t => t.HomeOwerId)
                .Index(t => t.BuildingId);
            
            AlterTableAnnotations(
                "dbo.localink_HomeOwerDoors",
                c => new
                    {
                        HomeOwerId = c.Long(nullable: false),
                        DoorId = c.Long(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_HomeOwerDoor_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.localink_HomeOwers", "FlatNo", c => c.String());
            AddColumn("dbo.localink_OpenAttemps", "DoorId", c => c.Long(nullable: false));
            AlterColumn("dbo.localink_Messages", "HomeOwerId", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.localink_HomeOwerDoors", new[] { "HomeOwerId", "DoorId" });
            CreateIndex("dbo.localink_AccessKeys", "DoorId");
            CreateIndex("dbo.localink_AccessKeys", "HomeOwerId");
            CreateIndex("dbo.localink_Messages", "HomeOwerId");
            CreateIndex("dbo.localink_OpenAttemps", "DoorId");
            CreateIndex("dbo.localink_HomeOwerDoors", "DoorId");
            AddForeignKey("dbo.localink_AccessKeys", "DoorId", "dbo.localink_Doors", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_AccessKeys", "HomeOwerId", "dbo.localink_HomeOwers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_HomeOwerDoors", "DoorId", "dbo.localink_Doors", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_OpenAttemps", "DoorId", "dbo.localink_Doors", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers", "Id", cascadeDelete: true);
            DropColumn("dbo.localink_HomeOwerDoors", "Id");
            DropColumn("dbo.localink_HomeOwerDoors", "TenantId");
            DropColumn("dbo.localink_HomeOwerDoors", "CreationTime");
            DropColumn("dbo.localink_HomeOwerDoors", "CreatorUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_HomeOwerDoors", "CreatorUserId", c => c.Long());
            AddColumn("dbo.localink_HomeOwerDoors", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.localink_HomeOwerDoors", "TenantId", c => c.Int());
            AddColumn("dbo.localink_HomeOwerDoors", "Id", c => c.Long(nullable: false, identity: true));
            DropForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers");
            DropForeignKey("dbo.localink_OpenAttemps", "DoorId", "dbo.localink_Doors");
            DropForeignKey("dbo.localink_Buildings", "CommunityId", "dbo.localink_Communities");
            DropForeignKey("dbo.localink_HomeOwerBuildings", "BuildingId", "dbo.localink_HomeOwers");
            DropForeignKey("dbo.localink_HomeOwerBuildings", "HomeOwerId", "dbo.localink_Buildings");
            DropForeignKey("dbo.localink_HomeOwerDoors", "DoorId", "dbo.localink_Doors");
            DropForeignKey("dbo.localink_AccessKeys", "HomeOwerId", "dbo.localink_HomeOwers");
            DropForeignKey("dbo.localink_AccessKeys", "DoorId", "dbo.localink_Doors");
            DropIndex("dbo.localink_HomeOwerBuildings", new[] { "BuildingId" });
            DropIndex("dbo.localink_HomeOwerBuildings", new[] { "HomeOwerId" });
            DropIndex("dbo.localink_HomeOwerDoors", new[] { "DoorId" });
            DropIndex("dbo.localink_OpenAttemps", new[] { "DoorId" });
            DropIndex("dbo.localink_Messages", new[] { "HomeOwerId" });
            DropIndex("dbo.localink_Buildings", new[] { "CommunityId" });
            DropIndex("dbo.localink_AccessKeys", new[] { "HomeOwerId" });
            DropIndex("dbo.localink_AccessKeys", new[] { "DoorId" });
            DropPrimaryKey("dbo.localink_HomeOwerDoors");
            AlterColumn("dbo.localink_Messages", "HomeOwerId", c => c.Long());
            DropColumn("dbo.localink_OpenAttemps", "DoorId");
            DropColumn("dbo.localink_HomeOwers", "FlatNo");
            AlterTableAnnotations(
                "dbo.localink_HomeOwerDoors",
                c => new
                    {
                        HomeOwerId = c.Long(nullable: false),
                        DoorId = c.Long(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_HomeOwerDoor_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            DropTable("dbo.localink_HomeOwerBuildings");
            DropTable("dbo.localink_Buildings",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Building_AdminCommunityFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Building_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Building_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            AddPrimaryKey("dbo.localink_HomeOwerDoors", "Id");
            CreateIndex("dbo.localink_OpenAttemps", "CommunityId");
            CreateIndex("dbo.localink_Messages", "HomeOwerId");
            CreateIndex("dbo.localink_HomeOwers", "CommunityId");
            CreateIndex("dbo.localink_AccessKeys", "CommunityId");
            AddForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers", "Id");
            AddForeignKey("dbo.localink_OpenAttemps", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_AccessKeys", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_HomeOwers", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
        }
    }
}
