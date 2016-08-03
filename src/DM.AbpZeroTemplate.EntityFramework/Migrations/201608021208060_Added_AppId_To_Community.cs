namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_AppId_To_Community : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.cms_Apps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        AppName = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(maxLength: 20),
                        AppDir = c.String(nullable: false, maxLength: 128),
                        AppUrl = c.String(nullable: false, maxLength: 1024),
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
                        "DynamicFilter_App_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_App_MustHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Channels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        ParentId = c.Long(),
                        Code = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        ChannelTemplateId = c.Long(nullable: false),
                        ContentTemplateId = c.Long(nullable: false),
                        IsIndex = c.Boolean(nullable: false),
                        ModelType = c.String(maxLength: 50),
                        ChannelGroupNameCollection = c.String(maxLength: 256),
                        ImageUrl = c.String(maxLength: 256),
                        FilePath = c.String(maxLength: 256),
                        LinkUrl = c.String(maxLength: 256),
                        LinkType = c.String(maxLength: 50),
                        Content = c.String(),
                        Keywords = c.String(maxLength: 50),
                        Description = c.String(maxLength: 50),
                        ContentNum = c.Int(nullable: false),
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
                        "DynamicFilter_Channel_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Channel_MustHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Contents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubTitle = c.String(maxLength: 128),
                        ImageUrl = c.String(maxLength: 256),
                        VideoUrl = c.String(maxLength: 256),
                        FileUrl = c.String(maxLength: 256),
                        LinkUrl = c.String(maxLength: 256),
                        Summary = c.String(maxLength: 256),
                        Author = c.String(maxLength: 50),
                        Source = c.String(maxLength: 50),
                        IsTop = c.Boolean(nullable: false),
                        IsRecommend = c.Boolean(nullable: false),
                        IsHot = c.Boolean(nullable: false),
                        IsColor = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        ChannelId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 128),
                        ContentText = c.String(),
                        ContentGroupNameCollection = c.String(maxLength: 256),
                        Tags = c.String(maxLength: 50),
                        IsChecked = c.Boolean(nullable: false),
                        CheckedLevel = c.Int(nullable: false),
                        Comments = c.Int(nullable: false),
                        Hits = c.Int(nullable: false),
                        HitsByDay = c.Int(nullable: false),
                        HitsByWeek = c.Int(nullable: false),
                        HitsByMonth = c.Int(nullable: false),
                        LastHitsDate = c.DateTime(),
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
                        "DynamicFilter_Content_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Content_MustHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Goods",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubTitle = c.String(maxLength: 128),
                        ImageUrl = c.String(maxLength: 256),
                        VideoUrl = c.String(maxLength: 256),
                        FileUrl = c.String(maxLength: 256),
                        LinkUrl = c.String(maxLength: 256),
                        Summary = c.String(maxLength: 256),
                        IsTop = c.Boolean(nullable: false),
                        IsRecommend = c.Boolean(nullable: false),
                        IsHot = c.Boolean(nullable: false),
                        IsColor = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        ChannelId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 128),
                        ContentText = c.String(),
                        ContentGroupNameCollection = c.String(maxLength: 256),
                        Tags = c.String(maxLength: 50),
                        IsChecked = c.Boolean(nullable: false),
                        CheckedLevel = c.Int(nullable: false),
                        Comments = c.Int(nullable: false),
                        Hits = c.Int(nullable: false),
                        HitsByDay = c.Int(nullable: false),
                        HitsByWeek = c.Int(nullable: false),
                        HitsByMonth = c.Int(nullable: false),
                        LastHitsDate = c.DateTime(),
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
                        "DynamicFilter_Good_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Good_MustHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Templates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Type = c.String(nullable: false, maxLength: 64),
                        Extension = c.String(nullable: false, maxLength: 64),
                        IsDefault = c.Boolean(nullable: false),
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
                        "DynamicFilter_Template_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Template_MustHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.localink_Communities", "AppId", c => c.Long(nullable: false));
            AlterColumn("dbo.cms_Apps", "TenantId", c => c.Int());
            AlterColumn("dbo.cms_Channels", "TenantId", c => c.Int());
            AlterColumn("dbo.cms_Contents", "TenantId", c => c.Int());
            AlterColumn("dbo.cms_Goods", "TenantId", c => c.Int());
            AlterColumn("dbo.cms_Templates", "TenantId", c => c.Int());
            CreateIndex("dbo.localink_Communities", "AppId");
            AddForeignKey("dbo.localink_Communities", "AppId", "dbo.cms_Apps", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_Communities", "AppId", "dbo.cms_Apps");
            DropIndex("dbo.localink_Communities", new[] { "AppId" });
            AlterColumn("dbo.cms_Templates", "TenantId", c => c.Int(nullable: false));
            AlterColumn("dbo.cms_Goods", "TenantId", c => c.Int(nullable: false));
            AlterColumn("dbo.cms_Contents", "TenantId", c => c.Int(nullable: false));
            AlterColumn("dbo.cms_Channels", "TenantId", c => c.Int(nullable: false));
            AlterColumn("dbo.cms_Apps", "TenantId", c => c.Int(nullable: false));
            DropColumn("dbo.localink_Communities", "AppId");
            AlterTableAnnotations(
                "dbo.cms_Templates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Type = c.String(nullable: false, maxLength: 64),
                        Extension = c.String(nullable: false, maxLength: 64),
                        IsDefault = c.Boolean(nullable: false),
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
                        "DynamicFilter_Template_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Template_MustHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Goods",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubTitle = c.String(maxLength: 128),
                        ImageUrl = c.String(maxLength: 256),
                        VideoUrl = c.String(maxLength: 256),
                        FileUrl = c.String(maxLength: 256),
                        LinkUrl = c.String(maxLength: 256),
                        Summary = c.String(maxLength: 256),
                        IsTop = c.Boolean(nullable: false),
                        IsRecommend = c.Boolean(nullable: false),
                        IsHot = c.Boolean(nullable: false),
                        IsColor = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        ChannelId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 128),
                        ContentText = c.String(),
                        ContentGroupNameCollection = c.String(maxLength: 256),
                        Tags = c.String(maxLength: 50),
                        IsChecked = c.Boolean(nullable: false),
                        CheckedLevel = c.Int(nullable: false),
                        Comments = c.Int(nullable: false),
                        Hits = c.Int(nullable: false),
                        HitsByDay = c.Int(nullable: false),
                        HitsByWeek = c.Int(nullable: false),
                        HitsByMonth = c.Int(nullable: false),
                        LastHitsDate = c.DateTime(),
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
                        "DynamicFilter_Good_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Good_MustHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Contents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubTitle = c.String(maxLength: 128),
                        ImageUrl = c.String(maxLength: 256),
                        VideoUrl = c.String(maxLength: 256),
                        FileUrl = c.String(maxLength: 256),
                        LinkUrl = c.String(maxLength: 256),
                        Summary = c.String(maxLength: 256),
                        Author = c.String(maxLength: 50),
                        Source = c.String(maxLength: 50),
                        IsTop = c.Boolean(nullable: false),
                        IsRecommend = c.Boolean(nullable: false),
                        IsHot = c.Boolean(nullable: false),
                        IsColor = c.Boolean(nullable: false),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        ChannelId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 128),
                        ContentText = c.String(),
                        ContentGroupNameCollection = c.String(maxLength: 256),
                        Tags = c.String(maxLength: 50),
                        IsChecked = c.Boolean(nullable: false),
                        CheckedLevel = c.Int(nullable: false),
                        Comments = c.Int(nullable: false),
                        Hits = c.Int(nullable: false),
                        HitsByDay = c.Int(nullable: false),
                        HitsByWeek = c.Int(nullable: false),
                        HitsByMonth = c.Int(nullable: false),
                        LastHitsDate = c.DateTime(),
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
                        "DynamicFilter_Content_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Content_MustHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Channels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        AppId = c.Long(nullable: false),
                        ParentId = c.Long(),
                        Code = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        ChannelTemplateId = c.Long(nullable: false),
                        ContentTemplateId = c.Long(nullable: false),
                        IsIndex = c.Boolean(nullable: false),
                        ModelType = c.String(maxLength: 50),
                        ChannelGroupNameCollection = c.String(maxLength: 256),
                        ImageUrl = c.String(maxLength: 256),
                        FilePath = c.String(maxLength: 256),
                        LinkUrl = c.String(maxLength: 256),
                        LinkType = c.String(maxLength: 50),
                        Content = c.String(),
                        Keywords = c.String(maxLength: 50),
                        Description = c.String(maxLength: 50),
                        ContentNum = c.Int(nullable: false),
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
                        "DynamicFilter_Channel_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Channel_MustHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.cms_Apps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        AppName = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(maxLength: 20),
                        AppDir = c.String(nullable: false, maxLength: 128),
                        AppUrl = c.String(nullable: false, maxLength: 1024),
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
                        "DynamicFilter_App_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_App_MustHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
        }
    }
}
