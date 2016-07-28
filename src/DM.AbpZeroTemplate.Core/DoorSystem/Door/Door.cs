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

namespace DM.AbpZeroTemplate.DoorSystem
{
    [Table("localink_Doors")]
    public class Door : FullAuditedEntity<long>, IMayHaveTenant
    {
        public Door()
        {

        }

        public Door(int? tenantId, string name, string pId, string departId)
        {
            TenantId = tenantId;
            Name = name;
            PId = pId;
            IsAuth = false;
            DepartId = departId;
        }

        public const int MaxDefaultStringLength = 50;


        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public virtual long CommunityId { get; set; }

        /// <summary>
        /// 小区
        /// </summary>
        [ForeignKey("CommunityId")]
        public virtual Community.Community Community { get; set; }

        /// <summary>
        /// 门禁名称
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 门禁类型
        /// </summary>
        public virtual string DoorType { get; set; }

        /// <summary>
        /// 门禁编号
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string PId { get; set; }

        /// <summary>
        /// 妙兜小区Id
        /// </summary>
        [StringLength(MaxDefaultStringLength)]
        public virtual string DepartId { get; set; }

        /// <summary>
        /// 是否认证
        /// </summary>
        public virtual bool IsAuth { get; private set; }

        /// <summary>
        /// 验证门禁，更新小区信息之前操作
        /// </summary>
        public void AuthDoor()
        {
            if (this.IsAuth)
                return;
            try
            {
                DoorClient dc = new DoorClient();
                var response = dc.Create(this.PId, this.DepartId, this.Name);
                if (response.Code == "0")
                {
                    this.IsAuth = true;
                }
                else
                {
                    this.IsAuth = false;
                }
            }
            catch (Exception)
            {
                this.IsAuth = false;
            }
        }
    }
}
