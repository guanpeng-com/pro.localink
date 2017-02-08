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
    //Model Dto Code About Delivery
    [AutoMapFrom(typeof(Delivery))]
    public class DeliveryDto : AuditedEntityDto<long>
    {
        public int TenantId;

        public string Title;

        public string Content;

        public long HomeOwerId;
        public long CommunityId;

        public string HomeOwerName;

        public string BuildingName;

        public bool IsGather;

        public string GatherTime;

        public bool IsReplace;

        public long ReplaceHomeOwerId;

        public string ReplaceHomeOwerName;

        public string Token;

        public string FlatNo;

        public string Barcode;

        public string CreationTimeStr;
    }
}
