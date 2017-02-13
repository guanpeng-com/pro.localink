using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About Report
    [AutoMapFrom(typeof(Report))]
    public class ReportDto : AuditedEntityDto<long>
    {
        public int TenantId;

        public string Title;

        public string Content;

        public long HomeOwerId;
        public long CommunityId;
        public long BuildingId;
        public long FlatNoId;


        public string HomeOwerName;
        public string CommunityName;
        public string BuildingName;
        public string FlatNo;

        public string Files;

        public List<string> FileArray;

        public string Status;

        public string CreationTimeStr;

        public string HandyMan;

        public string CompleteTimeStr;

    }
}
