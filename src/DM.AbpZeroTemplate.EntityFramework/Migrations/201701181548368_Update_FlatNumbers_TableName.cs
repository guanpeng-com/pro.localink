namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_FlatNumbers_TableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FlatNumbers", newName: "localink_FlatNumbers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.localink_FlatNumbers", newName: "FlatNumbers");
        }
    }
}
