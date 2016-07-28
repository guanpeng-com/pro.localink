using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Door
    public class CreateDoorInput : IInputDto
    {
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
