using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Gather Model Dto Code About Delivery
    public class GatherDeliveryInput : IInputDto
    {
        public long Id;

        /// <summary>
        /// 1-本人领取
        /// 2-代领
        /// </summary>
        public string Type;
    }
}
