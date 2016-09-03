using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DM.AbpZeroTemplate.WebApi.Models
{
    public class AuthUserSendCodeModel
    {
        /// <summary>
        /// 小区Id
        /// </summary>
        [Required]
        public long CommunityId { get; set; }

        /// <summary>
        /// 业主手机号
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int? TenantId { get; set; }

    }
}
