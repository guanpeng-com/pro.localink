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
using Abp.Runtime.Session;
using DM.AbpZeroTemplate.Authorization.Roles;
using DM.AbpZeroTemplate.Authorization.Users;
using Abp.Authorization;
using DM.AbpZeroTemplate.Authorization;

namespace DM.AbpZeroTemplate.DoorSystem.Community
{
    public class CommunityService : AbpZeroTemplateServiceBase, ICommunityService
    {
        private readonly CommunityManager _communityManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly IPermissionChecker _permissionChecker;

        public CommunityService(CommunityManager communityManager,
            RoleManager roleManager,
            IAbpSession abpSession,
            UserManager userManager,
            IPermissionChecker permissionChecker)
        {
            _communityManager = communityManager;
            _roleManager = roleManager;
            _abpSession = abpSession;
            _userManager = userManager;
            _permissionChecker = permissionChecker;
        }

        public async Task CreateCommunity(CreateCommunityInput input)
        {

            double lat = 0;
            double lng = 0;
            if (!string.IsNullOrEmpty(input.LatLng))
            {
                lat = double.Parse(input.LatLng.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                lng = double.Parse(input.LatLng.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }

            var community = new Community(CurrentUnitOfWork.GetTenantId(), input.Name, input.Address, lat, lng);
            if (input.DoorTypes != null)
                community.DoorTypes = String.Join(",", input.DoorTypes);
            if (input.Images != null)
                community.Images = String.Join(",", input.Images);

            await _communityManager.CreateAsync(community);
            CurrentUnitOfWork.SaveChanges();

            //设置当前用户的角色，有管理该小区的权限
            if (_abpSession.UserId.HasValue)
            {
                var currentUser = await _userManager.GetUserByIdAsync(_abpSession.UserId.Value);
                var distinctRoleIds = (
        from userListRoleDto in currentUser.Roles
        select userListRoleDto.RoleId
        ).Distinct();
                //角色管理的小区id
                if (await _permissionChecker.IsGrantedAsync(AppPermissions.Pages_DoorSystem_Communities_Create))
                {
                    foreach (var roleId in distinctRoleIds)
                    {
                        var role = (await _roleManager.GetRoleByIdAsync(roleId));
                        var communityIds = role.CommunityIdArray;
                        if (!communityIds.Contains(community.Id))
                        {
                            communityIds.Add(community.Id);
                        }
                        role.CommunityIdArray = communityIds;
                        await _roleManager.UpdateAsync(role);
                    }
                }
            }
        }

        public async Task ReCreateCMS(IdInput<long> input)
        {
            await _communityManager.ReCreateCMSAsync(input.Id);
        }

        public async Task DeleteCommunity(IdInput<long> input)
        {
            await _communityManager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<CommunityDto>> GetCommunities(GetCommunitiesInput input)
        {
            var query = _communityManager.FindCommunityList(input.Sorting);
            var allowIds = await GetAdminCommunityIdList();
            if (allowIds != null)
            {
                query = query.Where(c => allowIds.Contains(c.Id));
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(c => c.CreationTime).PageBy(input).ToListAsync();
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
            var lat = double.Parse(input.LatLng.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]);
            var lng = double.Parse(input.LatLng.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            var community = await _communityManager.CommunityRepository.GetAsync(input.Id);
            community.Name = input.Name;
            community.Address = input.Address;
            community.DoorTypes = String.Join(",", input.DoorTypes);
            community.Images = string.Join(",", input.Images);
            community.Lat = lat;
            community.Lng = lng;
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
            List<Community> list = new List<Community>();
            var allowIds = await GetAdminCommunityIdList();
            if (allowIds != null)
            {
                list = await (from c in _communityManager.CommunityRepository.GetAll()
                              where allowIds.Contains(c.Id)
                              select c)
                              .OrderByDescending(c => c.CreationTime)
                                .ToListAsync();
            }
            else
            {
                list = await (from c in _communityManager.CommunityRepository.GetAll()
                              select c)
                              .OrderByDescending(c => c.CreationTime)
                             .ToListAsync();
            }

            List<CommunityDto> result = new List<CommunityDto>();
            list.ForEach(item => result.Add(Mapper.Map<CommunityDto>(item)));
            return result;
        }
        public async Task<List<long>> GetAdminCommunityIdList()
        {
            List<long> allowIds = null;
            if (_abpSession.UserId.HasValue)
            {
                var currentUser = await _userManager.GetUserByIdAsync(_abpSession.UserId.Value);
                if (currentUser.UserName != User.AdminUserName)
                {
                    allowIds = new List<long>();
                    //用户角色
                    var distinctRoleIds = (
                            from userListRoleDto in currentUser.Roles
                            select userListRoleDto.RoleId
                            ).Distinct();
                    //角色管理的小区id
                    foreach (var roleId in distinctRoleIds)
                    {
                        var temp = (await _roleManager.GetRoleByIdAsync(roleId)).CommunityIdArray;
                        foreach (long item in temp)
                        {
                            if (!allowIds.Contains(item))
                            {
                                allowIds.Add(item);
                            }
                        }
                    }
                }
            }
            return allowIds;
        }
    }
}
