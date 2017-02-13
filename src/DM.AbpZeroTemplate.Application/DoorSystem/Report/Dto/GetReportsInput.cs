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
    //Create Model Dto Code About Report
    public class GetReportsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 业主Id
        /// </summary>
        public long? HomeOwerId;

        /// <summary>
        /// 关键字：BuildingName/FlatNo/Resident(HomeOwer)
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
        /// 状态：Draft/Sent
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public long? CommunityId { get; set; }

        /// <summary>
        /// 单元Id
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
