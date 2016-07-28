using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.AbpZeroTemplate.DoorSystem;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About Door
    [AutoMapFrom(typeof(Door))]
    public class DoorDto : AuditedEntityDto<long>
    {
        public long CommunityId;

        public string Name;

        public string PId;

        public string DepartId;

        public bool IsAuth;

        public string DoorType;

    }
}
