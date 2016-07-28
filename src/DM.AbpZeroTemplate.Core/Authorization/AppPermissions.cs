﻿namespace DM.AbpZeroTemplate.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";

        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";

        //CMS-SPECIFIC PERMISSIONS
        public const string Pages_CMS = "Pages.CMS";

        public const string Pages_CMS_Channels = "Pages.CMS.Channels";
        public const string Pages_CMS_Channels_Create = "Pages.CMS.Channels.Create";
        public const string Pages_CMS_Channels_Edit = "Pages.CMS.Channels.Edit";
        public const string Pages_CMS_Channels_Delete = "Pages.CMS.Channels.Delete";
        public const string Pages_CMS_Channels_Move = "Pages.CMS.Channels.Move";

        public const string Pages_CMS_Contents = "Pages.CMS.Contents";
        public const string Pages_CMS_Contents_Create = "Pages.CMS.Contents.Create";
        public const string Pages_CMS_Contents_Edit = "Pages.CMS.Contents.Edit";
        public const string Pages_CMS_Contents_Delete = "Pages.CMS.Contents.Delete";
        public const string Pages_CMS_Contents_Move = "Pages.CMS.Contents.Move";

        public const string Pages_CMS_Templates = "Pages.CMS.Templates";
        public const string Pages_CMS_Templates_Create = "Pages.CMS.Templates.Create";
        public const string Pages_CMS_Templates_Edit = "Pages.CMS.Templates.Edit";
        public const string Pages_CMS_Templates_Delete = "Pages.CMS.Templates.Delete";

        public const string Pages_CMS_Generate = "Pages.CMS.Generate";
        public const string Pages_CMS_Generate_Index = "Pages.CMS.Generate.Index";
        public const string Pages_CMS_Generate_Channel = "Pages.CMS.Generate.Channel";
        public const string Pages_CMS_Generate_Content = "Pages.CMS.Generate.Content";
        public const string Pages_CMS_Generate_File = "Pages.CMS.Generate.File";

        //DoorSystem
        public const string Pages_DoorSystem = "Pages.DoorSystem";
        public const string Pages_DoorSystem_Communities = "Pages.DoorSystem.Communities";
        public const string Pages_DoorSystem_Communities_Create = "Pages.DoorSystem.Communities.Create";
        public const string Pages_DoorSystem_Communities_Edit = "Pages.DoorSystem.Communities.Edit";
        public const string Pages_DoorSystem_Communities_Delete = "Pages.DoorSystem.Communities.Delete";
        public const string Pages_DoorSystem_Communities_Auth = "Pages.DoorSystem.Communities.Auth";

        public const string Pages_DoorSystem_HomeOwers = "Pages.DoorSystem.HomeOwers";
        public const string Pages_DoorSystem_HomeOwers_Create = "Pages.DoorSystem.HomeOwers.Create";
        public const string Pages_DoorSystem_HomeOwers_Edit = "Pages.DoorSystem.HomeOwers.Edit";
        public const string Pages_DoorSystem_HomeOwers_Delete = "Pages.DoorSystem.HomeOwers.Delete";

        public const string Pages_DoorSystem_Doors = "Pages.DoorSystem.Doors";
        public const string Pages_DoorSystem_Doors_Create = "Pages.DoorSystem.Doors.Create";
        public const string Pages_DoorSystem_Doors_Edit = "Pages.DoorSystem.Doors.Edit";
        public const string Pages_DoorSystem_Doors_Delete = "Pages.DoorSystem.Doors.Delete";
        public const string Pages_DoorSystem_Doors_Auth = "Pages.DoorSystem.Doors.Auth";

        public const string Pages_DoorSystem_AccessKeys = "Pages.DoorSystem.AccessKeys";
        public const string Pages_DoorSystem_AccessKeys_Create = "Pages.DoorSystem.AccessKeys.Create";
        public const string Pages_DoorSystem_AccessKeys_Edit = "Pages.DoorSystem.AccessKeys.Edit";
        public const string Pages_DoorSystem_AccessKeys_Delete = "Pages.DoorSystem.AccessKeys.Delete";
        public const string Pages_DoorSystem_AccessKeys_Auth = "Pages.DoorSystem.AccessKeys.Auth";


        public const string Pages_DoorSystem_Notices = "Pages.DoorSystem.Notices";
        public const string Pages_DoorSystem_Delivers = "Pages.DoorSystem.Delivers";
        public const string Pages_DoorSystem_Reports = "Pages.DoorSystem.Reports";
    }
}