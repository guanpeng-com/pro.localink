using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Update Model Dto Code About Report
    public class UpdateReportInput : IInputDto
    {
        public long Id;

        public string Title;

        public string Content;

        public long HomeOwerId;

        public List<string> FileArray;

        public string Status;

    }
}
