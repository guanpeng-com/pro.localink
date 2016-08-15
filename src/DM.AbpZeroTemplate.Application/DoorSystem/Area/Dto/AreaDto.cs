using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About Area
    [AutoMapFrom(typeof(Area))]
    public class AreaDto : AuditedEntityDto<long>
    {
        public long ParentId;

        public string Name;

        public string ParentPath;

        public List<AreaDto> Children;
    }
}
