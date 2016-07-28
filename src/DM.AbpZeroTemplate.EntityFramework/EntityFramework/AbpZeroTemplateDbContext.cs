﻿using System.Data.Common;
using System.Data.Entity;
using DM.AbpZeroTemplate.Authorization.Roles;
using DM.AbpZeroTemplate.Authorization.Users;
using DM.AbpZeroTemplate.MultiTenancy;
using DM.AbpZeroTemplate.Storage;
using DM.AbpZeroTemplate.CMS.DMUsers;
using Abp.CMS.EntityFramework;
using DM.AbpZeroTemplate.DoorSystem.Community;
using DM.AbpZeroTemplate.DoorSystem;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DM.AbpZeroTemplate.EntityFramework
{
    public class AbpZeroTemplateDbContext : AbpCMSDbContext<Tenant, Role, User, DMUser>
    {
        /* Define an IDbSet for each entity of the application */

        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }

        /// <summary>
        /// 小区
        /// </summary>
        public virtual IDbSet<Community> Communities { get; set; }

        /// <summary>
        /// 业主
        /// </summary>
        public virtual IDbSet<HomeOwer> HomeOwers { get; set; }

        /// <summary>
        /// 门禁
        /// </summary>
        public virtual IDbSet<Door> Doors { get; set; }

        /// <summary>
        /// 业主门禁
        /// </summary>
        public virtual IDbSet<HomeOwerDoor> HomeOwerDoors { get; set; }

        /// <summary>
        /// 钥匙
        /// </summary>
        public virtual IDbSet<AccessKey> AccessKeys { get; set; }

        /// <summary>
        ///  开门记录
        /// </summary>
        public virtual IDbSet<OpenAttemp> OpenAttemps { get; set; }

        /// <summary>
        ///  业主用户
        /// </summary>
        public virtual IDbSet<HomeOwerUser> HomeOwerUsers { get; set; }

        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public AbpZeroTemplateDbContext()
            : base("Default")
        {

        }

        /* This constructor is used by ABP to pass connection string defined in AbpZeroTemplateDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of AbpZeroTemplateDbContext since ABP automatically handles it.
         */
        public AbpZeroTemplateDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public AbpZeroTemplateDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //AbpCMSDbContext.InitDbSet(modelBuilder);
        }
    }
}