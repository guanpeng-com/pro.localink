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
        /// 添加业主信息
        /// ================================
        /// 1. 业主录入初始状态：Initial
        /// 2. 业主录入，默认自带小区大门的门禁
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateHomeOwer(CreateHomeOwerInput input);

        /// <summary>
        /// 修改业主消息
        /// ================================
        /// 1. 可以维护的字段：Forename, Surname, Phone, Email, Title, AltContact, AltMobile
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateHomeOwer(UpdateHomeOwerInput input);

        /// <summary>
        /// 删除业主
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
        /// 业主审核，同时发放钥匙
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AuthHomeOwer(IdInput<long> input);

        /// <summary>
        /// 锁定业主
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> LockHomeOwer(IdInput<long> input);

        /// <summary>
        /// 解锁业主
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UnLockHomeOwer(IdInput<long> input);

    }
}
