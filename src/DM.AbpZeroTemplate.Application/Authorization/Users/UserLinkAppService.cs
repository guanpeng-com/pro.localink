using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using DM.AbpZeroTemplate.Authorization.Users.Dto;
using DM.AbpZeroTemplate.MultiTenancy;

namespace DM.AbpZeroTemplate.Authorization.Users
{
    [AbpAuthorize]
    public class UserLinkAppService : AbpZeroTemplateAppServiceBase, IUserLinkAppService
    {
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserLinkAppService(
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            IUserLinkManager userLinkManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userLinkManager = userLinkManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task LinkToUser(LinkToUserInput input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var loginResult = await UserManager.LoginAsync(input.UsernameOrEmailAddress, input.Password, input.TenancyName);

                if (loginResult.Result != AbpLoginResultType.Success)
                {
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, input.UsernameOrEmailAddress, input.TenancyName);
                }

                if (loginResult.User.Id == AbpSession.GetUserId())
                {
                    throw new UserFriendlyException(L("YouCannotLinkToSameAccount"));
                }

                if (loginResult.User.ShouldChangePasswordOnNextLogin)
                {
                    throw new UserFriendlyException(L("ChangePasswordBeforeLinkToAnAccount"));
                }

                await _userLinkManager.Link(AbpSession.GetUserId(), loginResult.User.Id);
            }
        }

        public async Task<PagedResultOutput<LinkedUserDto>> GetLinkedUsers(GetLinkedUsersInput input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var query = CreateLinkedUsersQuery(input.Sorting)
                            .Skip(input.SkipCount)
                            .Take(input.MaxResultCount);

                var totalCount = await query.CountAsync();
                var linkedUsers = await query.ToListAsync();

                linkedUsers.ForEach(u =>
                {
                    u.TenancyName = GetTenancyNameById(u.TenantId);
                });

                return new PagedResultOutput<LinkedUserDto>(
                    totalCount,
                    linkedUsers
                );
            }
        }

        public async Task<ListResultOutput<LinkedUserDto>> GetRecentlyUsedLinkedUsers()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var query = CreateLinkedUsersQuery("LastLoginTime DESC");
                var recentlyUsedlinkedUsers = await query.Skip(0).Take(3).ToListAsync();

                recentlyUsedlinkedUsers.ForEach(u =>
                {
                    u.TenancyName = GetTenancyNameById(u.TenantId);
                });

                return new ListResultOutput<LinkedUserDto>(recentlyUsedlinkedUsers);
            }
        }

        public async Task UnlinkUser(UnlinkUserInput input)
        {
            var currentUserId = AbpSession.GetUserId();
            var currentUser = await UserManager.GetUserByIdAsync(currentUserId);

            if (!currentUser.UserLinkId.HasValue)
            {
                throw new ApplicationException(L("You are not linked to any account"));
            }

            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                if (!await _userLinkManager.AreUsersLinked(currentUserId, input.UserId))
                {
                    return;
                }

                await _userLinkManager.Unlink(input.UserId);
            }
        }

        private IQueryable<LinkedUserDto> CreateLinkedUsersQuery(string sorting)
        {
            var currentUserId = AbpSession.GetUserId();
            var currentUser = UserManager.Users.Single(u => u.Id == currentUserId);

            return UserManager.Users
                .Where(user => user.UserLinkId.HasValue && user.Id != currentUserId && user.UserLinkId == currentUser.UserLinkId)
                .OrderBy(sorting)
                .Select(user => new LinkedUserDto
                {
                    Id = user.Id,
                    ProfilePictureId = user.ProfilePictureId,
                    TenantId = user.TenantId,
                    Username = user.UserName
                });

            //return UserManager.Users
            //    .Join(TenantManager.Tenants, u => u.TenantId, t => t.Id, (u, t) => new { Id = u.Id, ProfilePictureId = u.ProfilePictureId, TenancyName = t.TenancyName, UserName = u.UserName, UserLinkId = u.UserLinkId, LastLoginTime = u.LastLoginTime })
            //    // .Include(user => user.TenantId)
            //    .Where(user => user.UserLinkId.HasValue && user.Id != currentUserId && user.UserLinkId == currentUser.UserLinkId)
            //    .OrderBy(sorting)
            //    .Select(user => new LinkedUserDto
            //    {
            //        Id = user.Id,
            //        ProfilePictureId = user.ProfilePictureId,
            //        TenancyName = user.TenancyName,
            //        Username = user.UserName
            //    });
        }

        [UnitOfWork]
        private string GetTenancyNameById(int? tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var tenant = TenantManager.Tenants.FirstOrDefault(t => t.Id == tenantId);
                return tenant == null ? null : tenant.TenancyName;
            }
        }

    }
}