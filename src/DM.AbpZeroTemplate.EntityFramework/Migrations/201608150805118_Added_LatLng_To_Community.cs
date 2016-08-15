namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_LatLng_To_Community : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_Communities", "Lat", c => c.Double(nullable: false));
            AddColumn("dbo.localink_Communities", "Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_Communities", "Lng");
            DropColumn("dbo.localink_Communities", "Lat");
        }
    }
}
