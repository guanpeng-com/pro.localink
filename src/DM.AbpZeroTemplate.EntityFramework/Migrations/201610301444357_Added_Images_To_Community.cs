namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Images_To_Community : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_Communities", "Images", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.localink_Communities", "Images");
        }
    }
}
