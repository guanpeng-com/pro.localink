﻿using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Runtime.Caching.Redis;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Hangfire;
using Microsoft.Owin.Security;
using DM.AbpZeroTemplate.Web.App.Startup;
using DM.AbpZeroTemplate.Web.Areas.Mpa.Startup;
using DM.AbpZeroTemplate.Web.Bundling;
using DM.AbpZeroTemplate.Web.Navigation;
using DM.AbpZeroTemplate.Web.Routing;
using DM.AbpZeroTemplate.WebApi;

namespace DM.AbpZeroTemplate.Web
{
    /// <summary>
    /// Web module of the application.
    /// This is the most top and entrance module that depends on others.
    /// </summary>
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(AbpZeroTemplateDataModule),
        typeof(AbpZeroTemplateApplicationModule),
        typeof(AbpZeroTemplateWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency can be removed if not using Redis cache
        typeof(AbpHangfireModule))] //AbpHangfireModule dependency can be removed if not using Hangfire
    public class AbpZeroTemplateWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database as language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();
            Configuration.Navigation.Providers.Add<FrontEndNavigationProvider>();
            Configuration.Navigation.Providers.Add<MpaNavigationProvider>();

            //Uncomment these lines to use HangFire as background job manager.
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});

            //Uncomment this line to use Redis cache instead of in-memory cache.
            //Configuration.Caching.UseRedis();
        }

        public override void Initialize()
        {
            //Dependency Injection
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
                );

            //Areas
            AreaRegistration.RegisterAllAreas();

            //Routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Bundling
            BundleTable.Bundles.IgnoreList.Clear();
            CommonBundleConfig.RegisterBundles(BundleTable.Bundles);
            AppBundleConfig.RegisterBundles(BundleTable.Bundles);
            FrontEndBundleConfig.RegisterBundles(BundleTable.Bundles);
            MpaBundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
            appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");
            appFolders.WebLogsFolder = server.MapPath("~/Logs");

            //普通上传文件路径
            appFolders.CommonFileFolder = server.MapPath("~/Upload/Common");
            //临时上传文件路径
            appFolders.TempFileFolder = server.MapPath("~/Upload/Temp");
            //应用普通文件上传路径
            appFolders.AppCommonFileFolder = server.MapPath("~/APP_PATH/Upload/Common");
            //应用临时文件上传路径
            appFolders.AppTempFileFolder = server.MapPath("~/APP_PATH/Upload/Temp");

            appFolders.AppFileFolder = "Upload/Files";
            appFolders.AppImageFolder = "Upload/Images";
            appFolders.AppVideoFolder = "Upload/Videos";

            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.CommonFileFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileFolder);
            }
            catch { }
        }
    }
}
