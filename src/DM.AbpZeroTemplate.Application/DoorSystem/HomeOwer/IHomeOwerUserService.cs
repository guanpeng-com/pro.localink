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
    //Domain Service Code About HomeOwerUser
    [AutoMapFrom(typeof(HomeOwerUser))]
    public interface IHomeOwerUserService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<HomeOwerUserDto>> GetHomeOwerUsers(GetHomeOwerUsersInput input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateHomeOwerUser(CreateHomeOwerUserInput input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateHomeOwerUser(UpdateHomeOwerUserInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteHomeOwerUser(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<HomeOwerUserDto> GetHomeOwerUser(IdInput<long> input);

    }
}
