using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Update Model Dto Code About Door
    public class UpdateDoorInput : IInputDto
    {
        [Required]
        public long Id;

        [Required]
        public long CommunityId;

        [Required]
        [MaxLength(HomeOwer.MaxDefaultStringLength)]
        public string Name;

        [Required]
        public string PId;
        
        public string DepartId;

        [Required]
        public string DoorType;
    }
}
