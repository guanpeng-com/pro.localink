namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Deliverys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.localink_Deliverys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(),
                        HomeOwerId = c.Long(nullable: false),
                        IsGather = c.Boolean(nullable: false),
                        GatherTime = c.DateTime(),
                        IsReplace = c.Boolean(nullable: false),
                        ReplaceHomeOwerId = c.Long(),
                        Token = c.String(),
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
                    { "DynamicFilter_Delivery_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Delivery_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_HomeOwers", t => t.HomeOwerId, cascadeDelete: true)
                .ForeignKey("dbo.localink_HomeOwers", t => t.ReplaceHomeOwerId)
                .Index(t => t.HomeOwerId)
                .Index(t => t.ReplaceHomeOwerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_Deliverys", "ReplaceHomeOwerId", "dbo.localink_HomeOwers");
            DropForeignKey("dbo.localink_Deliverys", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_Deliverys", new[] { "ReplaceHomeOwerId" });
            DropIndex("dbo.localink_Deliverys", new[] { "HomeOwerId" });
            DropTable("dbo.localink_Deliverys",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Delivery_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Delivery_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
