namespace DM.AbpZeroTemplate.Web.Navigation
{
    public static class PageNames
    {
        public static class App
        {
            public static class Common
            {
                public const string Administration = "Administration";
                public const string Roles = "Administration.Roles";
                public const string Users = "Administration.Users";
                public const string AuditLogs = "Administration.AuditLogs";
                public const string OrganizationUnits = "Administration.OrganizationUnits";
                public const string Languages = "Administration.Languages";
            }

            public static class Host
            {
                public const string Tenants = "Tenants";
                public const string Editions = "Editions";
                public const string Maintenance = "Administration.Maintenance";
                public const string Settings = "Administration.Settings.Host";
            }

            public static class Tenant
            {
                public const string Dashboard = "Dashboard.Tenant";
                public const string Settings = "Administration.Settings.Tenant";
            }

            public static class CMS
            {
                public const string CMSName = "CMS";
                public const string Channels = "CMS.Channels";
                public const string Contents = "CMS.Contents";
                public const string Templates = "CMS.Templates";
                public const string Generate = "CMS.Generate";
            }

            public static class DoorSystem
            {
                public const string DS = "DoorSystem";
                public const string Communities = "DoorSystem.Communities";
                public const string Doors = "DoorSystem.Doors";
                public const string HomeOwners = "DoorSystem.HomeOwners";
                public const string AccessKeys = "DoorSystem.AccessKeys";
                public const string OpenAttemps = "DoorSystem.OpenAttemps";
                public const string HomeOwerUsers = "DoorSystem.HomeOwerUsers";
                public const string Notices = "DoorSystem.Notices";
                public const string Deliverys = "DoorSystem.Deliverys";
                public const string Reports = "DoorSystem.Reports";
                public const string Messages = "DoorSystem.Messages";
            }
        }

        public static class Frontend
        {
            public const string Home = "Frontend.Home";
            public const string About = "Frontend.About";
        }
    }
}