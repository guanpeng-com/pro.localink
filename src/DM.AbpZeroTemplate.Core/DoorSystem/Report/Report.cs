﻿using Abp.Domain.Entities;
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
    /// <summary>
    /// 保修
    /// </summary>
    [Table("localink_Reports")]
    public class Report : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {
        #region 构造函数
        public Report() { }

        public Report(int? tenantId, string title, string content, long communityId)
        {
            TenantId = tenantId;
            Title = title;
            Content = content;
            Status = EReportStatusType.ReportSend;
            CommunityId = communityId;
        }
        #endregion

        #region 字段属性
        public const int MaxDefaultStringLength = 50;
        public const int MaxContentStringLength = 500;
        public const int MaxFilesStringLength = 1000;
        #endregion

        #region 外键
        /// <summary>
        /// 业主Id
        /// </summary>
        public virtual long HomeOwerId { get; set; }

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
        [StringLength(MaxContentStringLength)]
        public virtual string Content { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [StringLength(MaxContentStringLength)]
        public virtual string Files { get; private set; }
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
        /// 处理状态
        /// </summary>
        public virtual EReportStatusType Status { get; set; }

        /// <summary>
        /// 小区Id, 冗余字段
        /// </summary>
        public virtual long CommunityId { get; set; } 
        #endregion
    }
}
