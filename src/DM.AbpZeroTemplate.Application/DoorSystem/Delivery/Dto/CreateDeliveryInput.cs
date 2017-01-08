using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Delivery
    public class CreateDeliveryInput : IInputDto
    {

        public long HomeOwerId;

        public long BuildingId;

        public string Content;
    }
}
