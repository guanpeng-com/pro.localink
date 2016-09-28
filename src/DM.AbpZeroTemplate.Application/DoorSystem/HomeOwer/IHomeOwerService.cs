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
    //Domain Service Code About HomeOwer
    [AutoMapFrom(typeof(HomeOwer))]
    public interface IHomeOwerService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<HomeOwerDto>> GetHomeOwers(GetHomeOwersInput input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateHomeOwer(CreateHomeOwerInput input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateHomeOwer(UpdateHomeOwerInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteHomeOwer(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<HomeOwerDto> GetHomeOwer(IdInput<long> input);

        /// <summary>
        /// 获取业主状态
        /// </summary>
        /// <returns></returns>
        List<NameValueDto> GetAllHomeOwerStatus();

        /// <summary>
        /// 审核业主
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AuthHomeOwer(IdInput<long> input);
    }
}
