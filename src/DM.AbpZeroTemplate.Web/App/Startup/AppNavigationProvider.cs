﻿using Abp.Application.Navigation;
using Abp.Localization;
using DM.AbpZeroTemplate.Authorization;
using DM.AbpZeroTemplate.Web.Navigation;

namespace DM.AbpZeroTemplate.Web.App.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class AppNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Tenants,
                    L("Tenants"),
                    url: "host.tenants",
                    icon: "icon-globe",
                    requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Editions,
                    L("Editions"),
                    url: "host.editions",
                    icon: "icon-grid",
                    requiredPermissionName: AppPermissions.Pages_Editions
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Areas,
                    L("Areas"),
                    url: "host.areas",
                    icon: "icon-grid",
                    requiredPermissionName: AppPermissions.Pages_Areas
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Tenant.Dashboard,
                    L("Dashboard"),
                    url: "tenant.dashboard",
                    icon: "icon-home",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Dashboard
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Administration,
                    L("Administration"),
                    icon: "icon-wrench"
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OrganizationUnits,
                        L("OrganizationUnits"),
                        url: "organizationUnits",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Roles,
                        L("Roles"),
                        url: "roles",
                        icon: "icon-briefcase",
                        requiredPermissionName: AppPermissions.Pages_Administration_Roles
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Users,
                        L("Users"),
                        url: "users",
                        icon: "icon-users",
                        requiredPermissionName: AppPermissions.Pages_Administration_Users
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Languages,
                        L("Languages"),
                        url: "languages",
                        icon: "icon-flag",
                        requiredPermissionName: AppPermissions.Pages_Administration_Languages
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.AuditLogs,
                        L("AuditLogs"),
                        url: "auditLogs",
                        icon: "icon-lock",
                        requiredPermissionName: AppPermissions.Pages_Administration_AuditLogs
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Maintenance,
                        L("Maintenance"),
                        url: "host.maintenance",
                        icon: "icon-wrench",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Maintenance
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Settings,
                        L("Settings"),
                        url: "host.settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Settings
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Tenant.Settings,
                        L("Settings"),
                        url: "tenant.settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Tenant_Settings
                        )
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.CMS.CMSName,
                    L("CMS"),
                    icon: "icon-manage"
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.CMS.Channels,
                        L("Channels"),
                        url: "cms.channels",
                        icon: "icon-manage",
                        requiredPermissionName: AppPermissions.Pages_CMS_Channels
                        )
                   ).AddItem(new MenuItemDefinition(
                        PageNames.App.CMS.Contents,
                        L("Contents"),
                        url: "cms.contents",
                        icon: "icon-manage",
                        requiredPermissionName: AppPermissions.Pages_CMS_Contents
                        )
                   ).AddItem(new MenuItemDefinition(
                        PageNames.App.CMS.Templates,
                        L("Templates"),
                        url: "cms.templates",
                        icon: "icon-manage",
                        requiredPermissionName: AppPermissions.Pages_CMS_Templates
                        )
                   ).AddItem(new MenuItemDefinition(
                        PageNames.App.CMS.Generate,
                        L("Generate"),
                        url: "cms.generate",
                        icon: "icon-manage",
                        requiredPermissionName: AppPermissions.Pages_CMS_Generate
                        )
                   )
                 ).AddItem(
                    new MenuItemDefinition(
                        PageNames.App.DoorSystem.DS,
                        L("DoorSystem"),
                        icon: "icon-manage"
                     ).AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.Communities,
                                L("Communities"),
                                url: "doorSystem.communities",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_Communities
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.Doors,
                                L("Doors"),
                                url: "doorSystem.doors",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_Doors
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.HomeOwners,
                                L("HomeOwers"),
                                url: "doorSystem.homeOwers",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_HomeOwers
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.AccessKeys,
                                L("AccessKeys"),
                                url: "doorSystem.accessKeys",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_AccessKeys
                             )
                         ).AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.OpenAttemps,
                                L("OpenAttemps"),
                                url: "doorSystem.openAttemps",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_OpenAttemps
                             )
                         )
                         .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.Messages,
                                L("Messages"),
                                url: "doorSystem.messages",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_Messages
                            )
                         ).AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.Deliverys,
                                L("Delivers"),
                                url: "doorSystem.deliverys",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_Deliverys
                            )
                         )
                         .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.Reports,
                                L("Reports"),
                                url: "doorSystem.reports",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_Reports
                            )
                         )
                         .AddItem(
                            new MenuItemDefinition(
                                PageNames.App.DoorSystem.KeyHoldings,
                                L("KeyHoldings"),
                                url: "doorSystem.keyHoldings",
                                icon: "icon-manage",
                                requiredPermissionName: AppPermissions.Pages_DoorSystem_KeyHoldings
                            )
                         )
                     );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroTemplateConsts.LocalizationSourceName);
        }
    }
}
