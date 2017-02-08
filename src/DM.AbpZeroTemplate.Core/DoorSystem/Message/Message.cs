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

        public Message(int? tenantId, string title, string content, long communityId, long buildingId, long flatNoId, string communityName, string buildingName, string flatNo)
        {
            TenantId = tenantId;
            Title = title;
            Content = content;
            IsRead = false;
            CommunityId = communityId;
            BuildingId = buildingId;
            FlatNoId = flatNoId;
            CommunityName = communityName;
            BuildingName = buildingName;
            FlatNo = flatNo;
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

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public virtual string Files { get; set; }
        [NotMapped]
        public virtual List<string> FileArray
        {
            get
            {
                if (!string.IsNullOrEmpty(Files))
                {
                    return Files.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                return new List<string>();
            }
            set
            {
                if (value != null)
                    Files = String.Join(";", value);
            }
        }

        /// <summary>
        /// 是否已读，只针对私信
        /// </summary>
        public virtual bool IsRead { get; set; }

        /// <summary>
        /// 是否是公告，公告针对Building；私信针对HomeOwer
        /// </summary>
        public virtual bool IsPublic { get; set; }

        /// <summary>
        /// 消息状态：草稿/已发送
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// 小区Id, 冗余字段
        /// </summary>
        public virtual long CommunityId { get; set; }

        /// <summary>
        ///  单元Id, 冗余字段
        /// </summary>
        public virtual long BuildingId { get; set; }

        /// <summary>
        /// 门牌号Id，冗余字段
        /// </summary>
        public virtual long FlatNoId { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public virtual string CommunityName { get; set; }

        /// <summary>
        /// 单元
        /// </summary>
        public virtual string BuildingName { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public virtual string FlatNo { get; set; }
        #endregion
    }
}
