using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Community.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Community
{
    /// <summary>
    /// 小区领域服务
    /// </summary>
    public interface ICommunityService : IApplicationService
    {
        /// <summary>
        /// 获取小区列表，分页
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<CommunityDto>> GetCommunities(GetCommunitiesInput input);

        /// <summary>
        /// 创建小区
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateCommunity(CreateCommunityInput input);

        /// <summary>
        /// 更新小区
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateCommunity(UpdateCommunityInput input);

        /// <summary>
        /// 删除小区
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteCommunity(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CommunityDto> GetCommunity(IdInput<long> input);

        /// <summary>
        /// 验证小区
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CommunityDto> AuthCommunity(IdInput<long> input);


        /// <summary>
        /// 获取用户管理小区
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<CommunityDto>> GetUserCommunities();
    }
}
