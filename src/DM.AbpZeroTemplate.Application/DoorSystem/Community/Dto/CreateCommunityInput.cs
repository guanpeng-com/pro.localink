using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Community.Dto
{
    public class CreateCommunityInput : IInputDto
    {
        [Required]
        [StringLength(Community.MaxDefaultStringLength)]
        public string Name { get; set; }


        [StringLength(Community.MaxAddressStringLength)]
        public string Address { get; set; }

        public string[] DoorTypes { get; set; }

        /// <summary>
        /// 经度，纬度
        /// </summary>
        public string LatLng { get; set; }
    }
}
