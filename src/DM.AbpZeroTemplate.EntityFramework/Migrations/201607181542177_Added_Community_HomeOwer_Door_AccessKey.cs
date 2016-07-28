namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Community_HomeOwer_Door_AccessKey : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.dm_UserLoginAttempts", newName: "localink_UserLoginAttempts");
            RenameTable(name: "dbo.dm_UserLogins", newName: "localink_UserLogins");
            RenameTable(name: "dbo.dm_Users", newName: "localink_Users");
            CreateTable(
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
                    { "DynamicFilter_AccessKey_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AccessKey_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.localink_Doors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        HomeOwerId = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        DoorType = c.String(),
                        PId = c.String(nullable: false, maxLength: 50),
                        DepartId = c.String(nullable: false, maxLength: 50),
                        IsAuth = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Door_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Door_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
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
                    { "DynamicFilter_HomeOwer_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_HomeOwer_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            AlterTableAnnotations(
                "dbo.localink_Communities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 100),
                        IsAuth = c.Boolean(nullable: false),
                        DepartId = c.String(maxLength: 50),
                        DoorTypes = c.String(),
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
                        "DynamicFilter_Community_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Community_MustHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.localink_Communities", "DoorTypes", c => c.String());
            AlterColumn("dbo.localink_Communities", "TenantId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.localink_Communities", "TenantId", c => c.Int(nullable: false));
            DropColumn("dbo.localink_Communities", "DoorTypes");
            AlterTableAnnotations(
                "dbo.localink_Communities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 100),
                        IsAuth = c.Boolean(nullable: false),
                        DepartId = c.String(maxLength: 50),
                        DoorTypes = c.String(),
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
                        "DynamicFilter_Community_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Community_MustHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            DropTable("dbo.localink_HomeOwers",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_HomeOwer_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_HomeOwer_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.localink_Doors",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Door_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Door_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.localink_AccessKeys",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AccessKey_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AccessKey_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            RenameTable(name: "dbo.localink_Users", newName: "dm_Users");
            RenameTable(name: "dbo.localink_UserLogins", newName: "dm_UserLogins");
            RenameTable(name: "dbo.localink_UserLoginAttempts", newName: "dm_UserLoginAttempts");
        }
    }
}
