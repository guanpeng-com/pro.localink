using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About AccessKey
    public class CreateAccessKeyInput : IInputDto
    {
        [Required]
        public long DoorId;

        [Required]
        public long HomeOwerId;

        [Required]
        public DateTime Validity;

        public string LockId;

        public bool IsAuth;

    }
}
