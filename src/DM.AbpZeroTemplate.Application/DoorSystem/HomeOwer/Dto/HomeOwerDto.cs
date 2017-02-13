using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About HomeOwer
    [AutoMapFrom(typeof(HomeOwer))]
    public class HomeOwerDto : AuditedEntityDto<long>
    {
        public long CommunityId;

        public string CommunityName;

        public string HomeOwerName;

        public string Forename;

        public string Surname;

        public string Phone;

        public string Email;

        public string Title;

        public string BuildingName;

        public long BuildingId;

        public string FlatNo;

        public long FlatNoId;

        public string AltContact;

        public string AltMobile;

        public string CreationTimeStr;
        public string AccessStatus;
        public string UserGroup;
        public string Status;
    }
}
