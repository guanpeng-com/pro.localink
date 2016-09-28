using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DM.AbpZeroTemplate.WebApi.Controllers.v1.KeyHoldingsController;
using Swashbuckle.Swagger.Annotations;

namespace DM.AbpZeroTemplate.WebApi.Models
{
    public class CreateKeyHoldingModel
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
        /// 访客姓名
        /// </summary>
        [Required]
        public string VisitorName { get; set; }


        /// <summary>
        /// 允许到访开始时间
        /// </summary>
        [Required]
        public DateTime VisiteStartTime { get; set; }


        /// <summary>
        /// 允许到访结束时间
        /// </summary>
        [Required]
        public DateTime VisiteEndTime { get; set; }

        /// <summary>
        /// 钥匙类型
        /// </summary>
        public List<EDoorType> KeyTypes { get; set; }

        /// <summary>
        /// 到访令牌
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int? TenantId { get; set; }

    }
}
