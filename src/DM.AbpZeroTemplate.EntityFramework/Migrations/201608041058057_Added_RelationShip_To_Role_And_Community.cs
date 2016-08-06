namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_RelationShip_To_Role_And_Community : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleCommunities",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Community_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Community_Id })
                .ForeignKey("dbo.AbpRoles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.localink_Communities", t => t.Community_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Community_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleCommunities", "Community_Id", "dbo.localink_Communities");
            DropForeignKey("dbo.RoleCommunities", "Role_Id", "dbo.AbpRoles");
            DropIndex("dbo.RoleCommunities", new[] { "Community_Id" });
            DropIndex("dbo.RoleCommunities", new[] { "Role_Id" });
            DropTable("dbo.RoleCommunities");
        }
    }
}
