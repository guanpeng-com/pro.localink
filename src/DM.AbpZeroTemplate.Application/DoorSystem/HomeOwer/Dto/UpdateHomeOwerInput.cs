using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Update Model Dto Code About HomeOwer
    public class UpdateHomeOwerInput : IInputDto
    {


        public long Id;

        public long CommunityId;

        public string Forename;

        public string Surname;

        public string Phone;

        public string Email;

        public string Title;

        public string AltContact;

        public string AltMobile;

        //public long GarageId;

        //public long MailboxId;

        //public long MaindoorId;
    }
}
