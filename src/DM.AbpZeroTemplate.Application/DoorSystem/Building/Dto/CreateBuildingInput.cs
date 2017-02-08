using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Building
    public class CreateBuildingInput : IInputDto
    {
        /// <summary>
        /// 单元楼
        /// </summary>
        public string BuildingName;

        /// <summary>
        /// 小区Id
        /// </summary>
        public long CommunityId;
    }
}
