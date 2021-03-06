﻿/* 'app' MODULE DEFINITION */
var appModule = angular.module("app", [
    "ui.router",
    "ui.bootstrap",
    'ui.utils',
    "ui.jq",
    'ui.grid',
    'ui.grid.pagination',
    "oc.lazyLoad",
    "ngSanitize",
    'angularFileUpload',
    'daterangepicker',
    'angularMoment',
    'frapontillo.bootstrap-switch',
    'abp',
    'simditor',
    'datetimepicker',
    'monospaced.qrcode',
    'uiGmapgoogle-maps'
]);

/* LAZY LOAD CONFIG */

/*Google map config*/
appModule.config(['uiGmapGoogleMapApiProvider', function (GoogleMapApi) {
    GoogleMapApi.configure({
        key: 'AIzaSyDaYYEyeWe3Km0fGg3x-y1IHhMqdNeSTVI',

        libraries: 'weather,geometry,visualization,places'
    });
}]);


/* This application does not define any lazy-load yet but you can use $ocLazyLoad to define and lazy-load js/css files.
 * This code configures $ocLazyLoad plug-in for this application.
 * See it's documents for more information: https://github.com/ocombe/ocLazyLoad
 */
appModule.config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        cssFilesInsertBefore: 'ng_load_plugins_before', // load the css files before a LINK element with this ID.
        debug: false,
        events: true,
        modules: []
    });
}]);

/* THEME SETTINGS */
App.setAssetsPath(abp.appPath + 'metronic/assets/');
appModule.factory('settings', ['$rootScope', function ($rootScope) {
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        },
        layoutImgPath: App.getAssetsPath() + 'admin/layout4/img/',
        layoutCssPath: App.getAssetsPath() + 'admin/layout4/css/',
        assetsPath: abp.appPath + 'metronic/assets',
        globalPath: abp.appPath + 'metronic/assets/global',
        layoutPath: abp.appPath + 'metronic/assets/layouts/layout4'
    };

    $rootScope.settings = settings;

    return settings;
}]);

/* ROUTE DEFINITIONS */

appModule.config([
    '$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {

        // Default route (overrided below if user has permission)
        $urlRouterProvider.otherwise("/welcome");

        //Welcome page
        $stateProvider.state('welcome', {
            url: '/welcome',
            templateUrl: '~/App/common/views/welcome/index.cshtml'
        });

        //COMMON routes

        if (abp.auth.hasPermission('Pages.Administration.Roles')) {
            $stateProvider.state('roles', {
                url: '/roles',
                templateUrl: '~/App/common/views/roles/index.cshtml',
                menu: 'Administration.Roles'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Users')) {
            $stateProvider.state('users', {
                url: '/users?filterText',
                templateUrl: '~/App/common/views/users/index.cshtml',
                menu: 'Administration.Users'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Languages')) {
            $stateProvider.state('languages', {
                url: '/languages',
                templateUrl: '~/App/common/views/languages/index.cshtml',
                menu: 'Administration.Languages'
            });

            if (abp.auth.hasPermission('Pages.Administration.Languages.ChangeTexts')) {
                $stateProvider.state('languageTexts', {
                    url: '/languages/texts/:languageName?sourceName&baseLanguageName&targetValueFilter&filterText',
                    templateUrl: '~/App/common/views/languages/texts.cshtml',
                    menu: 'Administration.Languages'
                });
            }
        }

        if (abp.auth.hasPermission('Pages.Administration.AuditLogs')) {
            $stateProvider.state('auditLogs', {
                url: '/auditLogs',
                templateUrl: '~/App/common/views/auditLogs/index.cshtml',
                menu: 'Administration.AuditLogs'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.OrganizationUnits')) {
            $stateProvider.state('organizationUnits', {
                url: '/organizationUnits',
                templateUrl: '~/App/common/views/organizationUnits/index.cshtml',
                menu: 'Administration.OrganizationUnits'
            });
        }

        $stateProvider.state('notifications', {
            url: '/notifications',
            templateUrl: '~/App/common/views/notifications/index.cshtml'
        });

        //HOST routes

        $stateProvider.state('host', {
            'abstract': true,
            url: '/host',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.Tenants')) {
            $urlRouterProvider.otherwise("/host/tenants"); //Entrance page for the host
            $stateProvider.state('host.tenants', {
                url: '/tenants?filterText',
                templateUrl: '~/App/host/views/tenants/index.cshtml',
                menu: 'Tenants'
            });
        }

        if (abp.auth.hasPermission('Pages.Editions')) {
            $stateProvider.state('host.editions', {
                url: '/editions',
                templateUrl: '~/App/host/views/editions/index.cshtml',
                menu: 'Editions'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Host.Maintenance')) {
            $stateProvider.state('host.maintenance', {
                url: '/maintenance',
                templateUrl: '~/App/host/views/maintenance/index.cshtml',
                menu: 'Administration.Maintenance'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Host.Settings')) {
            $stateProvider.state('host.settings', {
                url: '/settings',
                templateUrl: '~/App/host/views/settings/index.cshtml',
                menu: 'Administration.Settings.Host'
            });
        }

        if (abp.auth.hasPermission('Pages.Areas')) {
            $stateProvider.state('host.areas', {
                url: '/areas',
                templateUrl: '~/App/host/views/areas/index.cshtml',
                menu: 'Areas'
            });
        }


        //TENANT routes

        $stateProvider.state('tenant', {
            'abstract': true,
            url: '/tenant',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.Tenant.Dashboard')) {
            $urlRouterProvider.otherwise("/tenant/dashboard"); //Entrance page for a tenant
            $stateProvider.state('tenant.dashboard', {
                url: '/dashboard',
                templateUrl: '~/App/tenant/views/dashboard/index.cshtml',
                menu: 'Dashboard.Tenant'
            });
        }

        if (abp.auth.hasPermission('Pages.Administration.Tenant.Settings')) {
            $stateProvider.state('tenant.settings', {
                url: '/settings',
                templateUrl: '~/App/tenant/views/settings/index.cshtml',
                menu: 'Administration.Settings.Tenant'
            });
        }

        //cms channel
        $stateProvider.state('cms', {
            'abstract': true,
            url: '/cms',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.CMS.Channels')) {
            $stateProvider.state('cms.channels', {
                url: '/channels',
                templateUrl: '~/App/cms/views/channels/index.cshtml',
                menu: 'CMS.Channels'
            });
        }

        if (abp.auth.hasPermission('Pages.CMS.Contents')) {
            $stateProvider.state('cms.contents', {
                url: '/contents',
                templateUrl: '~/App/cms/views/contents/index.cshtml',
                menu: 'CMS.Contents'
            });
        }

        if (abp.auth.hasPermission('Pages.CMS.Templates')) {
            $stateProvider.state('cms.templates', {
                url: '/templates',
                templateUrl: '~/App/cms/views/templates/index.cshtml',
                menu: 'CMS.Templates'
            });
        }

        if (abp.auth.hasPermission('Pages.CMS.Generate')) {
            $stateProvider.state('cms.generate', {
                url: '/generate',
                templateUrl: '~/App/cms/views/generate/index.cshtml',
                menu: 'CMS.Generate'
            })
            .state('cms.generate.webIndex', {
                url: '/webIndex?v=' + new Date().getTime(),
                templateUrl: '~/App/cms/views/generate/webIndex.cshtml',
                menu: 'CMS.Generate.Index'
            })
            .state('cms.generate.channel', {
                url: '/channel',
                templateUrl: '~/App/cms/views/generate/channel.cshtml',
                menu: 'CMS.Generate.Channel'
            })
            .state('cms.generate.content', {
                url: '/content',
                templateUrl: '~/App/cms/views/generate/content.cshtml',
                menu: 'CMS.Generate.Content'
            })
            .state('cms.generate.file', {
                url: '/file',
                templateUrl: '~/App/cms/views/generate/file.cshtml',
                menu: 'CMS.Generate.File'
            });
        }

        //doorSystem communities
        $stateProvider.state('doorSystem', {
            'abstract': true,
            url: '/doorSystem',
            template: '<div ui-view class="fade-in-up"></div>'
        });

        if (abp.auth.hasPermission('Pages.DoorSystem.Communities')) {
            $stateProvider.state('doorSystem.communities', {
                url: '/communities',
                templateUrl: '~/App/doorSystem/views/communities/index.cshtml',
                menu: 'DoorSystem.Communities'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.HomeOwers')) {
            $stateProvider.state('doorSystem.homeOwers', {
                url: '/homeOwers',
                templateUrl: '~/App/doorSystem/views/homeOwers/index.cshtml',
                menu: 'DoorSystem.HomeOwers'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.Doors')) {
            $stateProvider.state('doorSystem.homeOwerDoors', {
                url: '/:communityId/:homeOwerId/doors',
                templateUrl: '~/App/doorSystem/views/doors/homeOwerDoorIndex.cshtml',
                menu: 'DoorSystem.Doors'
            });
            $stateProvider.state('doorSystem.doors', {
                url: '/doors',
                templateUrl: '~/App/doorSystem/views/doors/index.cshtml',
                menu: 'DoorSystem.Doors'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.AccessKeys')) {
            $stateProvider.state('doorSystem.accessKeys', {
                url: '/accessKeys',
                templateUrl: '~/App/doorSystem/views/accessKeys/index.cshtml',
                menu: 'DoorSystem.AccessKeys'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.OpenAttemps')) {
            $stateProvider.state('doorSystem.openAttemps', {
                url: '/openAttemps',
                templateUrl: '~/App/doorSystem/views/openAttemps/index.cshtml',
                menu: 'DoorSystem.OpenAttemps'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.HomeOwerUsers')) {
            $stateProvider.state('doorSystem.homeOwerUsers', {
                url: '/homeOwerUsers',
                templateUrl: '~/App/doorSystem/views/homeOwerUsers/index.cshtml',
                menu: 'DoorSystem.HomeOwerUsers'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.Deliverys')) {
            $stateProvider.state('doorSystem.deliverys', {
                url: '/deliverys',
                templateUrl: '~/App/doorSystem/views/deliverys/index.cshtml',
                menu: 'DoorSystem.Deliverys'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.Reports')) {
            $stateProvider.state('doorSystem.reports', {
                url: '/reports',
                templateUrl: '~/App/doorSystem/views/reports/index.cshtml',
                menu: 'DoorSystem.Reports'
            });
        }
        if (abp.auth.hasPermission('Pages.DoorSystem.Messages')) {
            $stateProvider.state('doorSystem.messages', {
                url: '/messages',
                templateUrl: '~/App/doorSystem/views/messages/index.cshtml',
                menu: 'DoorSystem.Messages'
            });
        }

        if (abp.auth.hasPermission('Pages.DoorSystem.KeyHoldings')) {
            $stateProvider.state('doorSystem.keyHoldings', {
                url: '/keyHoldings',
                templateUrl: '~/App/doorSystem/views/keyHoldings/index.cshtml',
                menu: 'DoorSystem.KeyHoldings'
            });
        }
    }
]);

appModule.run(["$rootScope", "settings", "$state", 'i18nService', function ($rootScope, settings, $state, i18nService) {
    $rootScope.$state = $state;
    $rootScope.$settings = settings;

    //Set Ui-Grid language
    if (i18nService.get(abp.localization.currentCulture.name)) {
        i18nService.setCurrentLang(abp.localization.currentCulture.name);
    } else {
        i18nService.setCurrentLang("en");
    }

    $rootScope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
}]);