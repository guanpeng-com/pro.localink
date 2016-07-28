using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Community.Dto;
using Abp.AutoMapper;
using System.Data.Entity;
using Abp.Linq.Extensions;
using AutoMapper;

namespace DM.AbpZeroTemplate.DoorSystem.Community
{
    public class CommunityService : AbpZeroTemplateServiceBase, ICommunityService
    {
        private readonly CommunityManager _communityManager;

        public CommunityService(CommunityManager communityManager)
        {
            _communityManager = communityManager;
        }

        public async Task CreateCommunity(CreateCommunityInput input)
        {
            var community = new Community(CurrentUnitOfWork.GetTenantId(), input.Name, input.Address);
            community.DoorTypes = String.Join(",", input.DoorTypes);
            await _communityManager.CreateAsync(community);
        }

        public async Task DeleteCommunity(IdInput<long> input)
        {
            await _communityManager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<CommunityDto>> GetCommunities(GetCommunitiesInput input)
        {
            var query = _communityManager.FindCommunityList(input.Sorting);

            var totalCount = await query.CountAsync();
            var items = await query.PageBy(input).ToListAsync();
            return new PagedResultOutput<CommunityDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            var dto = item.MapTo<CommunityDto>();
                            return dto;
                        }
                    ).ToList()
                );
        }

        public async Task UpdateCommunity(UpdateCommunityInput input)
        {
            var community = await _communityManager.CommunityRepository.GetAsync(input.Id);
            community.Name = input.Name;
            community.Address = input.Address;
            community.DoorTypes = String.Join(",", input.DoorTypes);
            await _communityManager.UpdateAsync(community);
        }

        public async Task<CommunityDto> GetCommunity(IdInput<long> input)
        {
            return Mapper.Map<CommunityDto>(await _communityManager.CommunityRepository.GetAsync(input.Id));
        }

        public async Task<CommunityDto> AuthCommunity(IdInput<long> input)
        {
            var community = await _communityManager.CommunityRepository.GetAsync(input.Id);
            community.AuthCommunity();
            await _communityManager.UpdateAsync(community);
            return Mapper.Map<CommunityDto>(community);
        }

        /// <summary>
        /// 通过用户获取小区（管理员可以管理的小区）
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommunityDto>> GetUserCommunities()
        {
            var list = await (from c in _communityManager.CommunityRepository.GetAll()
                              select c)
                        .ToListAsync();

            //缺少用户权限验证

            List<CommunityDto> result = new List<CommunityDto>();
            list.ForEach(item => result.Add(Mapper.Map<CommunityDto>(item)));
            return result;
        }
    }
}
