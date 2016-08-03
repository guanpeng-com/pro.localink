using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.Common.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using System.Collections.Generic;
using Abp.Web.Models;
using System.Web.Http;
using DM.AbpZeroTemplate.HttpFormatters;

namespace DM.AbpZeroTemplate.Common
{
    public interface ICommonFileUploadAppService : IApplicationService
    {
        [DontWrapResult]
        Task<string> FileUpload(FileUpload<FileUploadInput> input);
    }
}