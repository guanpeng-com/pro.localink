using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using DM.AbpZeroTemplate.EntityFramework;
using DM.AbpZeroTemplate;

namespace DM.Localink.Migrator
{
    [DependsOn(typeof(AbpZeroTemplateDataModule))]
    public class LocalinkMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<AbpZeroTemplateDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}