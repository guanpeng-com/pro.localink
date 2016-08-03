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

        public string HomeOwerName;

        public string Files;

        public List<string> FileArray;

        public string Status;

    }
}
