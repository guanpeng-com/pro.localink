﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Update Model Dto Code About Building
    public class UpdateBuildingInput : IInputDto
    {
        public long Id;

        public long CommunityId;

        public string BuildingName;
    }
}
