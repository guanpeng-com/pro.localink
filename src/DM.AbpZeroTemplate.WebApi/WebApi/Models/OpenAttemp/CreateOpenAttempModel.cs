using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DM.AbpZeroTemplate.WebApi.Models
{
    public class CreateOpenAttempModel
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
        /// 是否成功
        /// </summary>
        [Required]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int? TenantId { get; set; }

    }
}
