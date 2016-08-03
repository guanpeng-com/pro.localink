using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using DM.AbpZeroTemplate.DoorSystem;
using DM.AbpZeroTemplate.DoorSystem.Community;
using DM.AbpZeroTemplate.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using DM.DoorSystem.Sdk;

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/HomeOwers")]
    public class HomeOwersController : AbpZeroTemplateApiControllerBase
    {
        private readonly CommunityManager _communityManager;
        private readonly AccessKeyManager _accessKeyManager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly DoorManager _doorManager;
        private readonly TenantManager _tenantManager;
        private readonly HomeOwerUserManager _homeOwerUserManager;

        public HomeOwersController(
            CommunityManager communityManager,
            AccessKeyManager accessKeyManager,
            HomeOwerManager homeOwerManager,
            DoorManager doorManager,
            TenantManager tenantManager,
            HomeOwerUserManager homeOwerUserManager)
            : base(homeOwerUserManager)
        {
            _communityManager = communityManager;
            _accessKeyManager = accessKeyManager;
            _homeOwerManager = homeOwerManager;
            _tenantManager = tenantManager;
            _doorManager = doorManager;
            _homeOwerUserManager = homeOwerUserManager;
        }

        /// <summary>
        ///  获取业主的钥匙
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="id">业主Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        [Route("/{id:long}/AccessKeys")]
        public virtual async Task<IHttpActionResult> GetAccessKeys(long id, string userName, string token, int? tenantId = null)
        {
            if (!base.AuthUser()) return Unauthorized();
            try
            {
                //var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
                //int? tenantId = tenant == null ? (int?)null : tenant.Id;
                using (CurrentUnitOfWork.SetTenantId(tenantId))
                {
                    var homeOwer = _homeOwerManager.HomeOwerRepository.FirstOrDefault(h => h.Id == id);
                    if (homeOwer == null)
                    {
                        return NotFound();
                    }
                    var homeOwerUser = await _homeOwerUserManager.HomeOwerUserRepository.FirstOrDefaultAsync(hu => hu.HomeOwerId == homeOwer.Id);
                    if (homeOwerUser == null)
                    {
                        throw new Exception("User is Unauthorized. HomeOwerUser is not exists.");
                    }

                    var query = (from a in _accessKeyManager.AccessKeyRepository.GetAll()
                                 join d in _doorManager.DoorRepository.GetAll() on a.DoorId equals d.Id
                                 where a.HomeOwerId == homeOwer.Id && a.IsAuth && d.IsAuth
                                 select new { KeyId = a.LockId, KeyValidity = a.Validity, CommunityId = d.DepartId, KeyName = d.Name }
                                ).ToList();

                    return Ok(new
                    {
                        AppKey = DM.DoorSystem.Sdk.DoorSystem.AppKey,
                        UserId = homeOwer.Phone,
                        AccessKeys = query
                    });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <returns></returns>
        [SecretVersionedRoute]
        [HttpPost]
        [UnitOfWork]
        [Route("/{id:long}/RegisterUserToHomeOwer")]
        public async virtual Task<IHttpActionResult> RegisterUserToHomeOwer(string userName, string token)
        {
            try
            {
                var homeOwerUser = new HomeOwerUser(userName, token);

                await _homeOwerUserManager.CreateAsync(homeOwerUser);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 认证用户为业主
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="communityId">小区Id</param>
        /// <param name="homeOwerName">业主姓名</param>
        /// <param name="phone">业主手机号</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [Route("/{id:long}/AuthUserToHomeOwer")]
        public async virtual Task<IHttpActionResult> AuthUserToHomeOwer(string userName, long communityId, string homeOwerName, string phone, string token, int? tenantId = null)
        {
            if (!base.AuthUser()) return Unauthorized();
            try
            {
                using (CurrentUnitOfWork.SetTenantId(tenantId))
                {
                    var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);
                    var homeOwer = await _homeOwerManager.GetHomeOwerByNameAndPhoneAndCommunityId(communityId, homeOwerName, phone);
                    if (homeOwerUser == null)
                    {
                        return NotFound();
                    }
                    if (homeOwer == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        homeOwerUser.HomeOwerId = homeOwer.Id;
                        homeOwerUser.TenantId = tenantId;
                        await _homeOwerUserManager.UpdateAsync(homeOwerUser);
                    }

                    return Ok();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
