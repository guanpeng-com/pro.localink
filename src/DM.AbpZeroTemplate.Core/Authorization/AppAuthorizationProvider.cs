using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace DM.AbpZeroTemplate.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: MultiTenancySides.Host);

            //CMS-SPECIFIC PERMISSIONS
            var cms = pages.CreateChildPermission(AppPermissions.Pages_CMS, L("CMS"));

            cms.CreateChildPermission(AppPermissions.Pages_CMS_Channels, L("Channels"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Channels_Create, L("ChannelCreate"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Channels_Delete, L("ChannelDelete"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Channels_Edit, L("ChannelEdit"));

            cms.CreateChildPermission(AppPermissions.Pages_CMS_Contents, L("Contents"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Contents_Create, L("ContentCreate"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Contents_Delete, L("ContentDelete"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Contents_Edit, L("ContentEdit"));

            cms.CreateChildPermission(AppPermissions.Pages_CMS_Templates, L("Templates"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Templates_Create, L("TemplateCreate"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Templates_Delete, L("TemplateDelete"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Templates_Edit, L("TemplateEdit"));

            cms.CreateChildPermission(AppPermissions.Pages_CMS_Generate, L("Generate"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Generate_Index, L("GenerateIndex"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Generate_Channel, L("GenerateChannel"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Generate_Content, L("GenerateContent"));
            cms.CreateChildPermission(AppPermissions.Pages_CMS_Generate_File, L("GenerateFile"));

            //DoorSystem-SPECIFIC PERMISSIONS
            var ds = pages.CreateChildPermission(AppPermissions.Pages_DoorSystem, L("DoorSystem"));

            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Communities, L("Communities"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Communities_Create, L("CommunitiesCreate"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Communities_Edit, L("CommunitiesEdit"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Communities_Delete, L("CommunitiesDelete"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Communities_Auth, L("CommunitiesAuth"));


            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwers, L("HomeOwers"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwers_Create, L("HomeOwersCreate"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwers_Edit, L("HomeOwersEdit"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwers_Delete, L("HomeOwersDelete"));


            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Doors, L("Doors"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Doors_Create, L("DoorsCreate"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Doors_Edit, L("DoorsEdit"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Doors_Delete, L("DoorsDelete"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Doors_Auth, L("DoorsAuth"));

            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_AccessKeys, L("AccessKeys"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_AccessKeys_Create, L("AccessKeysCreate"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_AccessKeys_Edit, L("AccessKeysEdit"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_AccessKeys_Delete, L("AccessKeysDelete"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_AccessKeys_Auth, L("AccessKeysAuth"));

            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_OpenAttemps, L("OpenAttemps"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_OpenAttemps_Delete, L("OpenAttempsDelete"));

            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwerUsers, L("HomeOwerUsers"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwerUsers_Create, L("HomeOwerUsersCreate"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwerUsers_Edit, L("HomeOwerUsersEdit"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_HomeOwerUsers_Delete, L("HomeOwerUsersDelete"));

            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Notices, L("Notices"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Delivers, L("Delivers"));
            ds.CreateChildPermission(AppPermissions.Pages_DoorSystem_Reports, L("Reports"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroTemplateConsts.LocalizationSourceName);
        }
    }
}
