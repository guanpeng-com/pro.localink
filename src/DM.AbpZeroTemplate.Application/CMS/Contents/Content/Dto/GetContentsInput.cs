using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using DM.AbpZeroTemplate.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.CMS.Contents.Dto
{
    //Create Model Dto Code About Report
    public class GetContentsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long? ChannelId;

        public List<int> SelectTypes;

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime";
            }
        }
    }
}
