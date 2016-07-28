using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About HomeOwer
    public class CreateHomeOwerInput : IInputDto
    {
        public long CommunityId;

        public string Name;

        public string Phone;

        public string Email;

        public string Gender;

        public long GarageId;

        public long MailboxId;

        public long MaindoorId;

    }
}
