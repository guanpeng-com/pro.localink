using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Community.Dto
{
    [AutoMapFrom(typeof(Community))]
    public class CommunityDto : AuditedEntityDto<long>
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 租户公司Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 小区地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 是否验证
        /// </summary>
        public bool IsAuth { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lng { get; set; }
    }
}
