using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Update Model Dto Code About Message
    public class UpdateMessageInput : IInputDto
    {
        public long Id;

        public string Title;

        public string Content;

        public List<string> Files;

        public string Status;

    }
}
