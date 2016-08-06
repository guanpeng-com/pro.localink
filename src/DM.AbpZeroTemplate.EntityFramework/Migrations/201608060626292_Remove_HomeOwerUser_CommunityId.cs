namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_HomeOwerUser_CommunityId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.localink_HomeOwerUsers", "CommunityId", "dbo.localink_Communities");
            DropIndex("dbo.localink_HomeOwerUsers", new[] { "CommunityId" });
            AlterTableAnnotations(
                "dbo.localink_HomeOwerUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        HomeOwerId = c.Long(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Token = c.String(maxLength: 2000),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_HomeOwerUser_AdminCommunityFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            DropColumn("dbo.localink_HomeOwerUsers", "CommunityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_HomeOwerUsers", "CommunityId", c => c.Long(nullable: false));
            AlterTableAnnotations(
                "dbo.localink_HomeOwerUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        HomeOwerId = c.Long(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Token = c.String(maxLength: 2000),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_HomeOwerUser_AdminCommunityFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            CreateIndex("dbo.localink_HomeOwerUsers", "CommunityId");
            AddForeignKey("dbo.localink_HomeOwerUsers", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
        }
    }
}
