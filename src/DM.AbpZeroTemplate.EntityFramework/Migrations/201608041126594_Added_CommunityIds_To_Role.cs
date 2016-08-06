namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_CommunityIds_To_Role : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleCommunities", "Role_Id", "dbo.AbpRoles");
            DropForeignKey("dbo.RoleCommunities", "Community_Id", "dbo.localink_Communities");
            DropIndex("dbo.RoleCommunities", new[] { "Role_Id" });
            DropIndex("dbo.RoleCommunities", new[] { "Community_Id" });
            DropTable("dbo.RoleCommunities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoleCommunities",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Community_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Community_Id });
            
            CreateIndex("dbo.RoleCommunities", "Community_Id");
            CreateIndex("dbo.RoleCommunities", "Role_Id");
            AddForeignKey("dbo.RoleCommunities", "Community_Id", "dbo.localink_Communities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoleCommunities", "Role_Id", "dbo.AbpRoles", "Id", cascadeDelete: true);
        }
    }
}
