using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About Message
    [AutoMapFrom(typeof(Message))]
    public class MessageDto : AuditedEntityDto<long>
    {
        public int TenantId;

        public string Title;

        public string Content;

        public long HomeOwerId;
        public long CommunityId;

        public string HomeOwerName;

        public bool IsRead;

        public bool IsPublic;

    }
}
