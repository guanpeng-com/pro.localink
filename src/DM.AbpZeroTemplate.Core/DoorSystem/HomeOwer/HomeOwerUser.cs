﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
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
    [Table("localink_HomeOwerUsers")]
    public class HomeOwerUser : FullAuditedEntity<long>, IMayHaveTenant
    {
        public HomeOwerUser()
        {

        }

        public HomeOwerUser(int? tenantId, long homOwerId, string userName)
        {
            TenantId = tenantId;
            HomOwerId = homOwerId;
            UserName = userName;
        }

        public const int MaxDefaultStringLength = 50;


        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        public virtual long HomOwerId { get; set; }

        /// <summary>
        /// 业主
        /// </summary>
        [ForeignKey("HomOwerId")]
        public virtual HomeOwer HomOwer { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string UserName { get; set; }
    }
}