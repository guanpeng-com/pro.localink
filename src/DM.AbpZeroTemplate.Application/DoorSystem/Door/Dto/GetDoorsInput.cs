﻿using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using DM.AbpZeroTemplate.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Door
	    public class GetDoorsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 小区Id
        /// </summary>
        public long? CommunityId;

        /// <summary>
        /// 小区Id
        /// </summary>
        public long? HomeOwerId;

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }
        }
    }
}
