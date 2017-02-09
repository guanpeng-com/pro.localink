using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DM.AbpZeroTemplate.DoorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About Building
    [AutoMapFrom(typeof(Building))]
    public class BuildingDto : AuditedEntityDto<long>
    {
        public int TenantId;

        public long CommunityId;

        public string BuildingName;
    }
}
