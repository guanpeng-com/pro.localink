namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_HomeOwer_Door : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.localink_HomeOwerDoors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        HomeOwerId = c.Long(nullable: false),
                        DoorId = c.Long(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_HomeOwerDoor_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_HomeOwers", t => t.HomeOwerId, cascadeDelete: true)
                .Index(t => t.HomeOwerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_HomeOwerDoors", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_HomeOwerDoors", new[] { "HomeOwerId" });
            DropTable("dbo.localink_HomeOwerDoors",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_HomeOwerDoor_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
