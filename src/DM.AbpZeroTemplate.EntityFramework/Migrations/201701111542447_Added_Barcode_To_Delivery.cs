namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Barcode_To_Delivery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_Deliverys", "BuildingName", c => c.String());
            AddColumn("dbo.localink_Deliverys", "Barcode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_Deliverys", "Barcode");
            DropColumn("dbo.localink_Deliverys", "BuildingName");
        }
    }
}
