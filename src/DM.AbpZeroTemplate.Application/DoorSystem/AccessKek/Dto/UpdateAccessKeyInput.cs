using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Update Model Dto Code About AccessKey
    public class UpdateAccessKeyInput : IInputDto
    {
        [Required]
        public long Id;

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
