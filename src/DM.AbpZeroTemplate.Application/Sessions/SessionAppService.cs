using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using DM.AbpZeroTemplate.Sessions.Dto;
using DM.AbpZeroTemplate.DoorSystem.Community;

namespace DM.AbpZeroTemplate.Sessions
{
    [AbpAuthorize]
    public class SessionAppService : AbpZeroTemplateAppServiceBase, ISessionAppService
    {
        private readonly CommunityManager _communityManager;

        public SessionAppService(CommunityManager communityManager)
        {
            _communityManager = communityManager;
        }


        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                User = (await GetCurrentUserAsync()).MapTo<UserLoginInfoDto>(),
                App = (await GetCurrentAppAsync()).MapTo<AppLoginInfoDto>()
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            }

            return output;
        }

        /// <summary>
        /// 更新用户选择管理的CMS
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [DisableAuditing]
        public async Task SaveCurrentAppId(int communityId)
        {
            var user = await GetCurrentUserAsync();
            var community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(c => c.Id == communityId);
            if (community != null)
            {
                if (community.AppId != 0)
                {
                    user.AppId = community.AppId;
                    await UserManager.UpdateAsync(user);
                }
            }

        }
    }
}