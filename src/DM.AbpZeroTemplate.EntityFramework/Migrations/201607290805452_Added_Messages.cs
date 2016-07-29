namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Messages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.localink_Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(),
                        HomeOwerId = c.Long(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Message_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Message_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_HomeOwers", t => t.HomeOwerId, cascadeDelete: true)
                .Index(t => t.HomeOwerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_Messages", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_Messages", new[] { "HomeOwerId" });
            DropTable("dbo.localink_Messages",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Message_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Message_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
