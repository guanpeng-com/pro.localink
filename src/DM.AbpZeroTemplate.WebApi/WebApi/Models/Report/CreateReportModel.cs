using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DM.AbpZeroTemplate.WebApi.Controllers.v1.ReportsController;
using Swashbuckle.Swagger.Annotations;

namespace DM.AbpZeroTemplate.WebApi.Models
{
    public class CreateReportModel
    {
        /// <summary>
        /// 小区Id
        /// </summary>
        [Required]
        public long CommunityId { get; set; }

        /// <summary>
        /// 单元Id
        /// </summary>
        [Required]
        public long BuildingId { get; set; }

        /// <summary>
        /// 门牌号Id
        /// </summary>
        [Required]
        public long FlatNoId { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        [Required]
        public long HomeOwerId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string Title { get; set; }


        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public List<string> Files { get; set; }

    }
}
