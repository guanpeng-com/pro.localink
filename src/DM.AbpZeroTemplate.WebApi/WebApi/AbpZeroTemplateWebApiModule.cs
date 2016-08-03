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
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using DM.AbpZeroTemplate.Common.Dto;
using DM.AbpZeroTemplate.HttpFormatters;

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

            //设置上传multipart/form-data
            Configuration.Modules.AbpWebApi().HttpConfiguration.Formatters.Add(new UploadMultipartMediaTypeFormatter<FileUploadInput>());

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
                    vc.Version("secret", "Localink Internal Api");
                });

                    //添加说明文档
                    c.IncludeXmlComments(GetXmlCommentsPath());

                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    //web api key
                    c.ApiKey("apiKey")
                        .Description("API Key Authentication")
                        .Name("apiKey")
                        .In("header");
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


            var actionAttr = apiDesc.ActionDescriptor.GetCustomAttributes<SecretVersionedRoute>().FirstOrDefault();
            if (actionAttr != null)
            {
                if (targetApiVersion != "secret")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                int targetVersion;
                targetApiVersion = targetApiVersion.TrimStart('v');

                if (attr.Version != 0 && int.TryParse(targetApiVersion, out targetVersion))
                {
                    return attr.Version == targetVersion;
                };

                return false;
            }
        }

        private static string GetXmlCommentsPath()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
            var commentsFile = Path.Combine(baseDirectory, "bin", commentsFileName);

            return commentsFile;
            //return String.Format(@"{0}\bin\SwaggerUi.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }

    }
}
