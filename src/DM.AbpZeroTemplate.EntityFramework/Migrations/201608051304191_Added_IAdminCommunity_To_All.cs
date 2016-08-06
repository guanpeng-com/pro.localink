namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_IAdminCommunity_To_All : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.localink_AccessKeys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        DoorId = c.Long(nullable: false),
                        HomeOwerId = c.Long(nullable: false),
                        Validity = c.DateTime(nullable: false),
                        LockId = c.String(),
                        IsAuth = c.Boolean(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
                        "DynamicFilter_AccessKey_AdminCommunityFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_HomeOwers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        CommunityId = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Gender = c.String(maxLength: 50),
                        ValidateCode = c.String(maxLength: 50),
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
                        "DynamicFilter_HomeOwer_AdminCommunityFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_Deliverys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(maxLength: 500),
                        HomeOwerId = c.Long(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Delivery_AdminCommunityFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_HomeOwerUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        HomeOwerId = c.Long(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
            
            AlterTableAnnotations(
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
                        CommunityId = c.Long(nullable: false),
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
                        "DynamicFilter_Message_AdminCommunityFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_OpenAttemps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HomeOwerId = c.Long(nullable: false),
                        HomeOwerName = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        BrowserInfo = c.String(maxLength: 255),
                        ClientIpAddress = c.String(maxLength: 64),
                        ClientName = c.String(maxLength: 128),
                        CreationTime = c.DateTime(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                        CommunityId = c.Long(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_OpenAttemp_AdminCommunityFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_Reports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(maxLength: 500),
                        HomeOwerId = c.Long(nullable: false),
                        Files = c.String(maxLength: 500),
                        Status = c.Byte(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
                        "DynamicFilter_Report_AdminCommunityFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.localink_AccessKeys", "CommunityId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Deliverys", "CommunityId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_HomeOwerUsers", "CommunityId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Messages", "CommunityId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_OpenAttemps", "CommunityId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Reports", "CommunityId", c => c.Long(nullable: false));
            CreateIndex("dbo.localink_AccessKeys", "CommunityId");
            CreateIndex("dbo.localink_HomeOwerUsers", "CommunityId");
            CreateIndex("dbo.localink_OpenAttemps", "CommunityId");
            AddForeignKey("dbo.localink_AccessKeys", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_HomeOwerUsers", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.localink_OpenAttemps", "CommunityId", "dbo.localink_Communities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_OpenAttemps", "CommunityId", "dbo.localink_Communities");
            DropForeignKey("dbo.localink_HomeOwerUsers", "CommunityId", "dbo.localink_Communities");
            DropForeignKey("dbo.localink_AccessKeys", "CommunityId", "dbo.localink_Communities");
            DropIndex("dbo.localink_OpenAttemps", new[] { "CommunityId" });
            DropIndex("dbo.localink_HomeOwerUsers", new[] { "CommunityId" });
            DropIndex("dbo.localink_AccessKeys", new[] { "CommunityId" });
            DropColumn("dbo.localink_Reports", "CommunityId");
            DropColumn("dbo.localink_OpenAttemps", "CommunityId");
            DropColumn("dbo.localink_Messages", "CommunityId");
            DropColumn("dbo.localink_HomeOwerUsers", "CommunityId");
            DropColumn("dbo.localink_Deliverys", "CommunityId");
            DropColumn("dbo.localink_AccessKeys", "CommunityId");
            AlterTableAnnotations(
                "dbo.localink_Reports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(maxLength: 500),
                        HomeOwerId = c.Long(nullable: false),
                        Files = c.String(maxLength: 500),
                        Status = c.Byte(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
                        "DynamicFilter_Report_AdminCommunityFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_OpenAttemps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HomeOwerId = c.Long(nullable: false),
                        HomeOwerName = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        BrowserInfo = c.String(maxLength: 255),
                        ClientIpAddress = c.String(maxLength: 64),
                        ClientName = c.String(maxLength: 128),
                        CreationTime = c.DateTime(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                        CommunityId = c.Long(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_OpenAttemp_AdminCommunityFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
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
                        CommunityId = c.Long(nullable: false),
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
                        "DynamicFilter_Message_AdminCommunityFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_HomeOwerUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        HomeOwerId = c.Long(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
            
            AlterTableAnnotations(
                "dbo.localink_Deliverys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(maxLength: 500),
                        HomeOwerId = c.Long(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Delivery_AdminCommunityFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_HomeOwers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        CommunityId = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Gender = c.String(maxLength: 50),
                        ValidateCode = c.String(maxLength: 50),
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
                        "DynamicFilter_HomeOwer_AdminCommunityFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.localink_AccessKeys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        DoorId = c.Long(nullable: false),
                        HomeOwerId = c.Long(nullable: false),
                        Validity = c.DateTime(nullable: false),
                        LockId = c.String(),
                        IsAuth = c.Boolean(nullable: false),
                        CommunityId = c.Long(nullable: false),
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
                        "DynamicFilter_AccessKey_AdminCommunityFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}
