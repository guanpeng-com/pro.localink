using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About OpenAttemp
    [AutoMapFrom(typeof(OpenAttemp))]
    public interface IOpenAttempService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<OpenAttempDto>> GetOpenAttemps(GetOpenAttempsInput input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOpenAttemp(CreateOpenAttempInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteOpenAttemp(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OpenAttemp> GetOpenAttemp(IdInput<long> input);

    }
}
