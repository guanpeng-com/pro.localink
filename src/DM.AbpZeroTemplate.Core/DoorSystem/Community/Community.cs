using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DM.DoorSystem.Sdk.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Community
{
    [Table("localink_Communities")]
    public class Community : FullAuditedEntity<long>, IMayHaveTenant
    {
        public Community()
        {

        }

        public Community(int? tenantId, string name, string address)
        {
            TenantId = TenantId;
            Name = name;
            Address = address;
            IsAuth = false;
            DepartId = string.Empty;
        }

        public const int MaxDefaultStringLength = 50;
        public const int MaxAddressStringLength = 100;


        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 小区地址
        /// </summary>
        [StringLength(MaxAddressStringLength)]
        public virtual string Address { get; set; }

        /// <summary>
        /// 是否认证
        /// </summary>
        public virtual bool IsAuth { get; private set; }

        /// <summary>
        /// 妙兜小区Id
        /// </summary>
        [StringLength(MaxDefaultStringLength)]
        public virtual string DepartId { get; private set; }

        /// <summary>
        /// 小区可用的钥匙类型
        /// </summary>
        public virtual string DoorTypes { get; set; }

        /// <summary>
        /// 门禁
        /// </summary>
        public virtual ICollection<Door> Doors { get; set; }

        /// <summary>
        /// 业主
        /// </summary>
        public virtual ICollection<HomeOwer> HomeOwers { get; set; }

        /// <summary>
        /// 验证小区，更新小区信息之前操作
        /// </summary>
        public void AuthCommunity()
        {
            if (this.IsAuth)
                return;
            try
            {
                CommunityClient cc = new CommunityClient();
                var response = cc.Create(this.Name, this.Address);
                if (response.Code == "0")
                {
                    this.IsAuth = true;
                    this.DepartId = response.Community.DepartID;
                }
                else
                {
                    this.IsAuth = false;
                    this.DepartId = string.Empty;
                }
            }
            catch (Exception)
            {
                this.IsAuth = false;
            }
        }
    }
}
