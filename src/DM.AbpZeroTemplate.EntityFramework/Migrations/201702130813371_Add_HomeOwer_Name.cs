namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_HomeOwer_Name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_HomeOwers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_HomeOwers", "Name");
        }
    }
}
