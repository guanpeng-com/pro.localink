using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DM.AbpZeroTemplate.WebApi.Models
{
    public class ApplyAccessKeyModel
    {
        /// <summary>
        /// 小区Id
        /// </summary>
        [Required]
        public long CommunityId { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        [Required]
        public long HomeOwerId { get; set; }

        /// <summary>
        /// 钥匙类型(0-社区大门,1-邮箱,2-住户门,3-车库)
        /// </summary>
        [Required]
        public EDoorType DoorType { get; set; }

        /// <summary>
        /// 钥匙截止日期
        /// </summary>
        [Required]
        public DateTime Vilidity { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int? TenantId { get; set; }

    }
}
