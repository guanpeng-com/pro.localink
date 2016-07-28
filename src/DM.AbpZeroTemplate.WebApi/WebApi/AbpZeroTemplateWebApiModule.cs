using System.Linq;
using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;
using Abp.WebApi.OData;
using Abp.WebApi.OData.Configuration;
using Swashbuckle.Application;
using DM.AbpZeroTemplate.DoorSystem.Community;
using DM.AbpZeroTemplate.DoorSystem;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Web.Http.Description;

namespace DM.AbpZeroTemplate.WebApi
{
    /// <summary>
    /// Web API layer of the application.
    /// </summary>
    [DependsOn(typeof(AbpWebApiModule), typeof(AbpWebApiODataModule), typeof(AbpZeroTemplateApplicationModule))]
    public class AbpZeroTemplateWebApiModule : AbpModule
    {
        public override void PreInitialize()
        {
            ConfigureOData();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Automatically creates Web API controllers for all application services of the application
            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(AbpZeroTemplateApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));

            ConfigureSwaggerUi(); //Remove this line to disable swagger UI.
        }

        private void ConfigureOData()
        {
            var builder = Configuration.Modules.AbpWebApiOData().ODataModelBuilder;

            //Configure your entities here... see documentation: http://www.aspnetboilerplate.com/Pages/Documents/OData-Integration
            builder.EntitySet<Community>("CommunitysOData");
            builder.EntitySet<Door>("DoorsOData");
            builder.EntitySet<HomeOwer>("HomeOwersOData");
            builder.EntitySet<AccessKey>("AccessKeysOData");
        }

        private void ConfigureSwaggerUi()
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    //c.SingleApiVersion("v1", "DM.AbpZeroTemplate.WebApi");

                    c.MultipleApiVersions(
                (apiDesc, targetApiVersion) => ResolveVersionSupportByRouteConstraint(apiDesc, targetApiVersion),
                (vc) =>
                {
                    vc.Version("v2", "DM.AbpZeroTemplate.WebApi V2");
                    vc.Version("v1", "DM.AbpZeroTemplate.WebApi V1");
                });

                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                })
                .EnableSwaggerUi();
        }

        private static bool ResolveVersionSupportByRouteConstraint(ApiDescription apiDesc, string targetApiVersion)
        {
            var attr = apiDesc.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<VersionedRoute>().FirstOrDefault();

            if (attr == null)
            {
                return false;
            }

            int targetVersion;
            targetApiVersion = targetApiVersion.TrimStart('v');

            if (int.TryParse(targetApiVersion, out targetVersion))
            {
                return attr.Version == targetVersion;
            };

            return true;
        }
    }
}
