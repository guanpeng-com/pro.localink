namespace DM.AbpZeroTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_HomeOwer_Props : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.localink_HomeOwers", "Forename", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.localink_HomeOwers", "Surname", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.localink_HomeOwers", "Title", c => c.Byte(nullable: false));
            AddColumn("dbo.localink_HomeOwers", "AltContact", c => c.String());
            AddColumn("dbo.localink_HomeOwers", "AltMobile", c => c.String());
            AddColumn("dbo.localink_HomeOwers", "IsLock", c => c.Boolean(nullable: false));
            AddColumn("dbo.localink_HomeOwers", "UserGroup", c => c.Byte(nullable: false));
            AddColumn("dbo.localink_Reports", "BuildingId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Reports", "FlatNoId", c => c.Long(nullable: false));
            AddColumn("dbo.localink_Reports", "CommunityName", c => c.String());
            AddColumn("dbo.localink_Reports", "BuildingName", c => c.String());
            AddColumn("dbo.localink_Reports", "FlatNo", c => c.String());
            AddColumn("dbo.localink_Reports", "HandyMan", c => c.String());
            AddColumn("dbo.localink_Reports", "CompleteTime", c => c.DateTime());
            AlterColumn("dbo.localink_Messages", "Status", c => c.Byte(nullable: false));
            DropColumn("dbo.localink_HomeOwers", "Name");
            DropColumn("dbo.localink_HomeOwers", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.localink_HomeOwers", "Gender", c => c.String(maxLength: 50));
            AddColumn("dbo.localink_HomeOwers", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.localink_Messages", "Status", c => c.String());
            DropColumn("dbo.localink_Reports", "CompleteTime");
            DropColumn("dbo.localink_Reports", "HandyMan");
            DropColumn("dbo.localink_Reports", "FlatNo");
            DropColumn("dbo.localink_Reports", "BuildingName");
            DropColumn("dbo.localink_Reports", "CommunityName");
            DropColumn("dbo.localink_Reports", "FlatNoId");
            DropColumn("dbo.localink_Reports", "BuildingId");
            DropColumn("dbo.localink_HomeOwers", "UserGroup");
            DropColumn("dbo.localink_HomeOwers", "IsLock");
            DropColumn("dbo.localink_HomeOwers", "AltMobile");
            DropColumn("dbo.localink_HomeOwers", "AltContact");
            DropColumn("dbo.localink_HomeOwers", "Title");
            DropColumn("dbo.localink_HomeOwers", "Surname");
            DropColumn("dbo.localink_HomeOwers", "Forename");
        }
    }
}
