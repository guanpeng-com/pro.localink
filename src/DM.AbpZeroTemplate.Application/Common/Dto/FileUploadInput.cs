using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DM.AbpZeroTemplate.Common.Dto
{
    public class FileUploadInput : IInputDto
    {
        public long MaxFileLength;
        public string AllowFileExtension;
        public List<string> AllowFileExtensionArray
        {
            get
            {
                List<string> list = new List<string>();
                list.AddRange(AllowFileExtension.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                return list;
            }
        }

        [Required]
        public string FileUploadType;

        public long? HomeOwerId;

        public long? CommunityId;
    }
}