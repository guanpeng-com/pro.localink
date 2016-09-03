namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_KeyHolding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.localink_KeyHoldings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        VisitorName = c.String(nullable: false, maxLength: 50),
                        VisiteTime = c.DateTime(nullable: false),
                        HomeOwerId = c.Long(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
                    { "DynamicFilter_KeyHolding_AdminCommunityFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_KeyHolding_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_KeyHolding_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_HomeOwers", t => t.HomeOwerId, cascadeDelete: true)
                .Index(t => t.HomeOwerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_KeyHoldings", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_KeyHoldings", new[] { "HomeOwerId" });
            DropTable("dbo.localink_KeyHoldings",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_KeyHolding_AdminCommunityFilter", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_KeyHolding_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_KeyHolding_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
