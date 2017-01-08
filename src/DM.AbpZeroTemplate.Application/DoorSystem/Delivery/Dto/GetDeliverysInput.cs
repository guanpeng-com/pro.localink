using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using DM.AbpZeroTemplate.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Delivery
    public class GetDeliverysInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 业主ID
        /// </summary>
        public long? HomeOwerId { get; set; }

        /// <summary>
        /// 关键字：Development/Resident(HomeOwer)
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 状态：Uncollected  Collected
        /// </summary>
        public bool? IsGather { get; set; }

        /// <summary>
        /// 单元楼Id
        /// </summary>
        public long? BuildingId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }
        }
    }
}
