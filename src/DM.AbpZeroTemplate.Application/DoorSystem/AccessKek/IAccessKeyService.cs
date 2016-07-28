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
    //Domain Service Code About AccessKey
    [AutoMapFrom(typeof(AccessKey))]
    public interface IAccessKeyService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<AccessKeyDto>> GetAccessKeys(GetAccessKeysInput input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAccessKey(CreateAccessKeyInput input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAccessKey(UpdateAccessKeyInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAccessKey(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AccessKeyDto> GetAccessKey(IdInput<long> input);

        /// <summary>
        /// 验证小区
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AccessKeyDto> AuthAccessKey(IdInput<long> input);
    }
}
