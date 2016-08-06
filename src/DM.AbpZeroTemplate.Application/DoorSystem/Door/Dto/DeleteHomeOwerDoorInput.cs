using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    public class DeleteHomeOwerDoorInput : IInputDto
    {
        [Required]
        public long DoorId;

        [Required]
        public long HomeOwerId;
    }
}
