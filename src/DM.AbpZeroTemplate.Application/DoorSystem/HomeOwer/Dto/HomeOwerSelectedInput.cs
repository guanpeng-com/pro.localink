using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{

    public class HomeOwerSelectedInput : IInputDto
    {
        public long CommunityId;
        public long BuildingId;
        public long FlatNoId;
        public long HomeOwerId;
    }
}
