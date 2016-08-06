using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About OpenAttemp
    [AutoMapFrom(typeof(OpenAttemp))]
    public class OpenAttempDto : AuditedEntityDto<long>
    {

        public long CommunityId;
        public long HomeOwerId;

        public string HomeOwerName;

        public string UserName;

        public string BowserInfo;

        public string ClientIpAddress;

        public string ClientName;

        public bool IsSuccess;

        public int TenantId;
        

    }
}
