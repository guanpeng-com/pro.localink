using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
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
    [Table("localink_OpenAttemps")]
    public class OpenAttemp : Entity<long>, IHasCreationTime, IMayHaveTenant, IAdminCommunity
    {
        public OpenAttemp() { }

        public OpenAttemp(int? tenantId, long communityId)
        {
            CreationTime = Clock.Now;
            TenantId = tenantId;
            CommunityId = communityId;
        }

        public const int MaxDefaultStringLength = 50;
        public const int MaxBrowserInfoLength = 255;
        public const int MaxClientIpAddressLength = 64;
        public const int MaxClientNameLength = 128;

        [Required]
        public virtual long HomeOwerId { get; set; }

        [Required]
        public virtual string HomeOwerName { get; set; }

        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string UserName { get; set; }

        [StringLength(MaxBrowserInfoLength)]
        public virtual string BrowserInfo { get; set; }

        [StringLength(MaxClientIpAddressLength)]
        public virtual string ClientIpAddress { get; set; }

        [StringLength(MaxClientNameLength)]
        public virtual string ClientName { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual bool IsSuccess { get; set; }

        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 门禁Id
        /// </summary>
        public virtual long DoorId { get; set; }

        /// <summary>
        ///  门禁
        /// </summary>
        [ForeignKey("DoorId")]
        public virtual Door Door { get; set; }


        public virtual long CommunityId { get; set; }
    }
}
