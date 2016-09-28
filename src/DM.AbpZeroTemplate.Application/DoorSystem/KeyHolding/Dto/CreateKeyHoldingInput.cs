using Abp.Application.Services.Dto;
using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About KeyHolding
    public class CreateKeyHoldingInput : IInputDto
    {
        public string VisitorName;

        public DateTime VisiteStartTime;

        public DateTime VisiteEndTime;

        public string Password;

        public EDoorType KeyType;

        public long HomeOwerId;

        public long CommunityId;

    }
}
