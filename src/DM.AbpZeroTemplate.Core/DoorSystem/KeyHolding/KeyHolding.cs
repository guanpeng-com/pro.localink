using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    [Table("localink_KeyHoldings")]
    public class KeyHolding : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {
        public KeyHolding()
        {
            IsCollection = false;
        }

        public KeyHolding(int? tenantId, string visitorName, DateTime visiteStartTime, DateTime visiteEndTime, string password, EDoorType keyType, long homeOwerId, long communityId)
        {
            TenantId = tenantId;
            VisitorName = visitorName;
            VisiteStartTime = visiteStartTime;
            VisiteEndTime = visiteEndTime;
            Password = password;
            KeyType = keyType;
            CommunityId = communityId;
            HomeOwerId = homeOwerId;
            IsCollection = false;
        }


        public const int MaxDefaultStringLength = 50;
        public const int MaxPasswordStringLength = 255;

        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 访客名称
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string VisitorName { get; set; }

        /// <summary>
        /// 用户访问令牌
        /// </summary>
        [StringLength(MaxPasswordStringLength)]
        public virtual string Password { get; set; }

        /// <summary>
        /// 允许到访开始时间
        /// </summary>
        [Required]
        public virtual DateTime VisiteStartTime { get; set; }

        /// <summary>
        /// 允许到访截止时间
        /// </summary>
        [Required]
        public virtual DateTime VisiteEndTime { get; set; }

        /// <summary>
        ///  访客到访时间
        /// </summary>
        public virtual DateTime? CollectionTime { get; set; }

        /// <summary>
        /// 是否到访
        /// </summary>
        public virtual bool IsCollection { get; set; }

        /// <summary>
        /// 业主ID
        /// </summary>
        public virtual long HomeOwerId { get; set; }

        /// <summary>
        /// 钥匙类型
        /// </summary>
        public virtual EDoorType KeyType { get; set; }

        [ForeignKey("HomeOwerId")]
        public virtual HomeOwer HomeOwer { get; set; }

        /// <summary>
        /// 小区ID
        /// </summary>
        public virtual long CommunityId { get; set; }
    }
}
