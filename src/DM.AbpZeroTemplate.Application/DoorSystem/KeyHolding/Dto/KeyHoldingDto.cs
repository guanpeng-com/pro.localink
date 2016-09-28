using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About KeyHolding
    [AutoMapFrom(typeof(KeyHolding))]
    public class KeyHoldingDto : AuditedEntityDto<long>
    {
        public int TenantId;

        public string VisitorName;

        public DateTime VisiteStartTime;

        public DateTime VisiteEndTime;

        /// <summary>
        /// CollectionTime
        /// </summary>
        public DateTime? CollectionTime;

        public bool IsCollection;

        public string Password;

        public EDoorType KeyType;

        public long HomeOwerId;

        public string HomeOwerName;

        public long CommunityId;

        public string CommunityName;

    }
}
