using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About HomeOwerUser
    [AutoMapFrom(typeof(HomeOwerUser))]
    public class HomeOwerUserDto : AuditedEntityDto<long>
    {
        public long HomeOwerId;

        public string HomeOwerName;

        public string UserName;

        public string Token;
    }
}
