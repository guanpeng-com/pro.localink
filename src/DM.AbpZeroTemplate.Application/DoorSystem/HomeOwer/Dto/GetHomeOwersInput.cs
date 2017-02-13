using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using DM.AbpZeroDoor.DoorSystem.Enums;
using DM.AbpZeroTemplate.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About HomeOwer
    public class GetHomeOwersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 业主状态
        /// </summary>
        public EHomeOwerStatusType? Status { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public long CommunityId { get; set; }

        /// <summary>
        ///  单元楼Id
        /// </summary>
        public long? BuildingId { get; set; }

        /// <summary>
        /// 关键字（Development / Resident）
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 业主类型：ManagingAgent/Owner/OwnerOccupier/Tenant
        /// </summary>
        public string UserGroup { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }
        }
    }
}
