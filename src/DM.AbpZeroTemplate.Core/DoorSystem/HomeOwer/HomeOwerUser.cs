using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using DM.AbpZeroTemplate.DoorSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    [MultiTenancySideAttribute(MultiTenancySides.Host)]
    [Table("localink_HomeOwerUsers")]
    public class HomeOwerUser : FullAuditedEntity<long>
    {
        public HomeOwerUser()
        {

        }

        public HomeOwerUser(string userName, string token)
        {
            Token = token;
            UserName = userName;
        }

        public const int MaxDefaultStringLength = 50;
        public const int MaxTokenStringLength = 2000;


        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        public virtual long HomeOwerId { get; set; }


        /// <summary>
        /// 用户
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 用户令牌
        /// </summary>
        [StringLength(MaxTokenStringLength)]
        public virtual string Token { get; set; }
    }
}
