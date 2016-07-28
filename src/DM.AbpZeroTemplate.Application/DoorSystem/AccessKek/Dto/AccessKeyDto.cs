using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About AccessKey
    [AutoMapFrom(typeof(AccessKey))]
    public class AccessKeyDto : AuditedEntityDto<long>
    {

        public long DoorId;

        public string DoorName;

        public long HomeOwerId;

        public string HomeOwerName;

        public DateTime Validity;

        public string LockId;

        public bool IsAuth;

    }
}
