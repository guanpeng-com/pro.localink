using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Report
    public class CreateReportInput : IInputDto
    {
        public string Title;

        public string Content;

        public long HomeOwerId;

        public List<string> FileArray;
    }
}
