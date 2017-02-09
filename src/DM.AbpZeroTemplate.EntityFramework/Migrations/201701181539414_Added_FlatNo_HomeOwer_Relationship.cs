namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_FlatNo_HomeOwer_Relationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.localink_HomeOwerBuildings", "BuildingId", "dbo.localink_Buildings");
            DropForeignKey("dbo.localink_HomeOwerBuildings", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_HomeOwerBuildings", new[] { "BuildingId" });
            DropIndex("dbo.localink_HomeOwerBuildings", new[] { "HomeOwerId" });
            CreateTable(
                "dbo.FlatNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BuildingId = c.Long(nullable: false),
                        CommunityId = c.Long(nullable: false),
                        TenantId = c.Int(),
                        FlatNo = c.String(),
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
                    { "DynamicFilter_FlatNumber_AdminCommunityFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_FlatNumber_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_FlatNumber_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_Buildings", t => t.BuildingId, cascadeDelete: true)
                .Index(t => t.BuildingId);
            
            CreateTable(
                "dbo.localink_HomeOwerFlatNos",
                c => new
                    {
                        FlatNoId = c.Long(nullable: false),
                        HomeOwerId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.FlatNoId, t.HomeOwerId })
                .ForeignKey("dbo.localink_HomeOwers", t => t.FlatNoId, cascadeDelete: true)
                .ForeignKey("dbo.FlatNumbers", t => t.HomeOwerId, cascadeDelete: true)
                .Index(t => t.FlatNoId)
                .Index(t => t.HomeOwerId);
            
            AddColumn("dbo.localink_Buildings", "HomeOwer_Id", c => c.Long());
            AddColumn("dbo.localink_Deliverys", "BuildingId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Deliverys", "FlatNoId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Deliverys", "FlatNo", c => c.String());
            AddColumn("dbo.localink_Messages", "Files", c => c.String());
            AddColumn("dbo.localink_Messages", "BuildingId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Messages", "FlatNoId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Messages", "BuildingName", c => c.String());
            AddColumn("dbo.localink_Messages", "FlatNo", c => c.String());
            CreateIndex("dbo.localink_Buildings", "HomeOwer_Id");
            AddForeignKey("dbo.localink_Buildings", "HomeOwer_Id", "dbo.localink_HomeOwers", "Id");
            DropColumn("dbo.localink_HomeOwers", "FlatNo");
            DropTable("dbo.localink_HomeOwerBuildings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.localink_HomeOwerBuildings",
                c => new
                    {
                        BuildingId = c.Long(nullable: false),
                        HomeOwerId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.BuildingId, t.HomeOwerId });
            
            AddColumn("dbo.localink_HomeOwers", "FlatNo", c => c.String());
            DropForeignKey("dbo.FlatNumbers", "BuildingId", "dbo.localink_Buildings");
            DropForeignKey("dbo.localink_HomeOwerFlatNos", "HomeOwerId", "dbo.FlatNumbers");
            DropForeignKey("dbo.localink_HomeOwerFlatNos", "FlatNoId", "dbo.localink_HomeOwers");
            DropForeignKey("dbo.localink_Buildings", "HomeOwer_Id", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_HomeOwerFlatNos", new[] { "HomeOwerId" });
            DropIndex("dbo.localink_HomeOwerFlatNos", new[] { "FlatNoId" });
            DropIndex("dbo.FlatNumbers", new[] { "BuildingId" });
            DropIndex("dbo.localink_Buildings", new[] { "HomeOwer_Id" });
            DropColumn("dbo.localink_Messages", "FlatNo");
            DropColumn("dbo.localink_Messages", "BuildingName");
            DropColumn("dbo.localink_Messages", "FlatNoId");
            DropColumn("dbo.localink_Messages", "BuildingId");
            DropColumn("dbo.localink_Messages", "Files");
            DropColumn("dbo.localink_Deliverys", "FlatNo");
            DropColumn("dbo.localink_Deliverys", "FlatNoId");
            DropColumn("dbo.localink_Deliverys", "BuildingId");
            DropColumn("dbo.localink_Buildings", "HomeOwer_Id");
            DropTable("dbo.localink_HomeOwerFlatNos");
            DropTable("dbo.FlatNumbers",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FlatNumber_AdminCommunityFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_FlatNumber_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_FlatNumber_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.localink_HomeOwerBuildings", "HomeOwerId");
            CreateIndex("dbo.localink_HomeOwerBuildings", "BuildingId");
            AddForeignKey("dbo.localink_HomeOwerBuildings", "HomeOwerId", "dbo.localink_HomeOwers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_HomeOwerBuildings", "BuildingId", "dbo.localink_Buildings", "Id", cascadeDelete: true);
        }
    }
}
