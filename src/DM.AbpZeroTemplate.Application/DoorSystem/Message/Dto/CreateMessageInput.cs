using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Message
    public class CreateMessageInput : IInputDto
    {
        public string Title;

        public string Content;

        public long HomeOwerId;

        public long CommunityId;

        public bool IsRead;

        public bool IsPublic;

    }
}
