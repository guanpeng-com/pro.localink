using System.Data.Common;
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
using EntityFramework.DynamicFilters;
using System.Collections.Generic;

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

        /// <summary>
        ///  业主消息
        /// </summary>
        public virtual IDbSet<Message> Messages { get; set; }

        /// <summary>
        ///  业主快递
        /// </summary>
        public virtual IDbSet<Delivery> Deliverys { get; set; }

        /// <summary>
        ///  业主报修
        /// </summary>
        public virtual IDbSet<Report> Reports { get; set; }

        /// <summary>
        ///  地区
        /// </summary>
        public virtual IDbSet<Area> Areas { get; set; }

        /// <summary>
        ///  访客记录
        /// </summary>
        public virtual IDbSet<KeyHolding> KeyHoldings { get; set; }

        /// <summary>
        /// 单元楼
        /// </summary>
        public virtual IDbSet<Building> Buildings { get; set; }

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
            modelBuilder.Filter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, (IAdminCommunity entity, ICollection<long> communityIds) => communityIds.Contains(entity.CommunityId), new List<long>());

            //自关联
            modelBuilder.Entity<Area>()
                                .HasMany(a => a.Children)
                                .WithOptional(a => a.Parent)
                                .HasForeignKey(a => a.ParentId);

            //门禁，钥匙，1 to M
            modelBuilder.Entity<Door>()
                                .HasMany<AccessKey>(d => d.AccessKeys)
                                .WithRequired(a => a.Door);

            //小区，单元楼，1 to M
            modelBuilder.Entity<Community>()
                                .HasMany(c => c.Buildings)
                                .WithRequired(b => b.Community);

            //单元楼，业主，M to M
            modelBuilder.Entity<Building>()
                                .HasMany(b => b.HomeOwers)
                                .WithMany(h => h.Buildings)
                                .Map(m=> {
                                    m.ToTable("localink_HomeOwerBuildings");
                                    m.MapLeftKey("BuildingId");
                                    m.MapRightKey("HomeOwerId");
                                });

            //业主，快递，1 to M
            modelBuilder.Entity<HomeOwer>()
                                .HasMany(h => h.Deliverys)
                                .WithRequired(d => d.HomeOwer);

            //业主，信息，1 to M
            modelBuilder.Entity<HomeOwer>()
                                .HasMany(h => h.Messages)
                                .WithRequired(d => d.HomeOwer);

            //业主，保修，1 to M
            modelBuilder.Entity<HomeOwer>()
                                .HasMany(h => h.Reports)
                                .WithRequired(d => d.HomeOwer);

            //业主，门禁，M to M
            modelBuilder.Entity<HomeOwer>()
                                .HasMany<Door>(h => h.Doors)
                                .WithMany(d => d.HomeOwers)
                                .Map(m =>
                                {
                                    m.ToTable("localink_HomeOwerDoors");
                                    m.MapLeftKey("DoorId");
                                    m.MapRightKey("HomeOwerId");
                                });

            //业主，钥匙，1 to M
            modelBuilder.Entity<HomeOwer>()
                                .HasMany(h => h.AccessKeys)
                                .WithRequired(a => a.HomeOwer);
        }
    }
}
