﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    /// <summary>
    /// 用户消息
    /// </summary>
    [Table("localink_Messages")]
    public class Message : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {
        #region 构造函数
        public Message() { }

        public Message(int? tenantId, string title, string content, long communityId)
        {
            TenantId = tenantId;
            Title = title;
            Content = content;
            IsRead = false;
            CommunityId = communityId;
        }
        #endregion

        #region 字段属性

        public const int MaxDefaultStringLength = 50;
        #endregion

        #region 外键
        /// <summary>
        /// 业主Id
        /// </summary>
        public virtual long? HomeOwerId { get; set; }

        [ForeignKey("HomeOwerId")]
        public virtual HomeOwer HomeOwer { get; set; }
        #endregion

        #region 基本信息
        public virtual int? TenantId { get; set; }

        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Title { get; set; }

        public virtual string Content { get; set; }

        public virtual bool IsRead { get; set; }

        public virtual bool IsPublic { get; set; }

        /// <summary>
        /// 小区Id, 冗余字段
        /// </summary>
        public virtual long CommunityId { get; set; } 
        #endregion
    }
}
