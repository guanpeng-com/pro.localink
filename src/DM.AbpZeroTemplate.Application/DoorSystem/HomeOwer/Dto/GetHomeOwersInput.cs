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
        public EHomeOwerStatusType? HomeOwerStatus { get; set; }

        /// <summary>
        ///  单元楼Id
        /// </summary>
        public long? BuildingId{ get; set; }

        /// <summary>
        /// 关键字（Development / Resident）
        /// </summary>
        public string Keywords { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }
        }
    }
}
