using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    [Table("localink_HomeOwerDoors")]
    public class HomeOwerDoor : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public HomeOwerDoor()
        {

        }

        public HomeOwerDoor(int? tenantId, long homeOwerId, long doorId)
        {
            TenantId = tenantId;
            HomeOwerId = homeOwerId;
            DoorId = doorId;
        }

        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        public virtual long HomeOwerId { get; set; }

        /// <summary>
        /// 门禁Id
        /// </summary>
        public virtual long DoorId { get; set; }
    }
}
