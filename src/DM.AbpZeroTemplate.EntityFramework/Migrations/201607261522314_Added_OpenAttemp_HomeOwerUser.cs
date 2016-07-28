namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_OpenAttemp_HomeOwerUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.localink_HomeOwerUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        HomOwerId = c.Long(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
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
                    { "DynamicFilter_HomeOwerUser_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_HomeOwerUser_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_HomeOwers", t => t.HomOwerId, cascadeDelete: true)
                .Index(t => t.HomOwerId);
            
            CreateTable(
                "dbo.localink_OpenAttemps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HomeOwerId = c.Long(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        BowserInfo = c.String(maxLength: 255),
                        ClientIpAddress = c.String(maxLength: 64),
                        ClientName = c.String(maxLength: 128),
                        CreationTime = c.DateTime(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OpenAttemp_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_HomeOwerUsers", "HomOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_HomeOwerUsers", new[] { "HomOwerId" });
            DropTable("dbo.localink_OpenAttemps",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OpenAttemp_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.localink_HomeOwerUsers",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_HomeOwerUser_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_HomeOwerUser_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
