namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Reports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
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
                    { "DynamicFilter_Report_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Report_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.localink_HomeOwers", t => t.HomeOwerId, cascadeDelete: true)
                .Index(t => t.HomeOwerId);
            
            AlterColumn("dbo.localink_Deliverys", "Content", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.localink_Reports", "HomeOwerId", "dbo.localink_HomeOwers");
            DropIndex("dbo.localink_Reports", new[] { "HomeOwerId" });
            AlterColumn("dbo.localink_Deliverys", "Content", c => c.String());
            DropTable("dbo.localink_Reports",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Report_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Report_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
