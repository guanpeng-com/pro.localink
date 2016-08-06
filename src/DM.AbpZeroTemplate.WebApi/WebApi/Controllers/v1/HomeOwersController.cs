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
using DM.DoorSystem.Sdk.Clients;
using DM.AbpZeroTemplate.Configuration;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using DM.AbpZeroTemplate.DoorSystem.Community.Dto;
using Abp.Web.Models;
using Abp.UI;

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/HomeOwers")]
    [WrapResult]
    public class HomeOwersController : AbpZeroTemplateApiControllerBase
    {
        private readonly CommunityManager _communityManager;
        private readonly AccessKeyManager _accessKeyManager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly DoorManager _doorManager;
        private readonly TenantManager _tenantManager;
        private readonly HomeOwerUserManager _homeOwerUserManager;

        private readonly DM.DoorSystem.Sdk.DoorSystem _doorSystemSdk = new DM.DoorSystem.Sdk.DoorSystem();

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
            base.AuthUser();
            //var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            //int? tenantId = tenant == null ? (int?)null : tenant.Id;
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwer = _homeOwerManager.HomeOwerRepository.FirstOrDefault(h => h.Id == id);
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                var homeOwerUser = await _homeOwerUserManager.HomeOwerUserRepository.FirstOrDefaultAsync(hu => hu.HomeOwerId == homeOwer.Id);
                if (homeOwerUser == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserNotExists);
                }

                var list = (from a in _accessKeyManager.AccessKeyRepository.GetAll()
                            join d in _doorManager.DoorRepository.GetAll() on a.DoorId equals d.Id
                            where a.HomeOwerId == homeOwer.Id && a.IsAuth && d.IsAuth
                            select new { KeyId = a.LockId, KeyValidity = a.Validity, CommunityId = d.DepartId, KeyName = d.Name }
                            ).ToList();

                return Ok(new
                {
                    AppKey = _doorSystemSdk.Params["app_key"],
                    UserId = homeOwer.Phone,
                    AccessKeys = list
                });
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
        [Route("/RegisterUserToHomeOwer")]
        public async virtual Task<IHttpActionResult> RegisterUserToHomeOwer(string userName, string token)
        {
            var homeOwerUser = new HomeOwerUser(userName, token);

            await _homeOwerUserManager.CreateAsync(homeOwerUser);

            return Ok();
        }

        /// <summary>
        /// 认证用户为业主，第一步，发送验证码
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="communityId">小区Id</param>
        /// <param name="phone">业主手机号</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [Route("/AuthUserSendCode")]
        public async virtual Task<IHttpActionResult> AuthUserSendCode(string userName, long communityId, string phone, string token, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);
                var homeOwer = await _homeOwerManager.GetHomeOwerByNameAndPhoneAndCommunityId(communityId, phone);
                if (homeOwerUser != null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserNotExists);
                }
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                else
                {
                    //发送验证码
                    SMSClient smsClient = new SMSClient();
                    var code = smsClient.ValidateCode();
                    var phoneCountryCode = await SettingManager.GetSettingValueAsync(AppSettings.UserManagement.PhoneCountryCode);
                    var response = smsClient.Send("Localink", phoneCountryCode + homeOwer.Phone, L("SMSValidateCode", code));
                    if (response.SMSSends.Count > 0 && response.SMSSends[0].Stuats == "0")
                    {
                        homeOwer.ValidateCode = code;
                        await _homeOwerManager.UpdateAsync(homeOwer);
                        return Ok();
                    }
                    else
                    {
                        throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.SMSSendCodeError);
                    }
                }
            }
        }

        /// <summary>
        /// 认证用户为业主，第二步，验证验证码，完成验证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="communityId"></param>
        /// <param name="phone"></param>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [Route("/AuthUserValidateCode")]
        public async virtual Task<IHttpActionResult> AuthUserValidateCode(string userName, long communityId, string phone, string token, string code, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);
                var homeOwer = await _homeOwerManager.GetHomeOwerByNameAndPhoneAndCommunityId(communityId, phone);
                if (homeOwerUser != null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserNotExists);
                }
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                else
                {
                    //验证验证码是否正确
                    if (!string.IsNullOrEmpty(homeOwer.ValidateCode) && code == homeOwer.ValidateCode)
                    {
                        homeOwerUser.HomeOwerId = homeOwer.Id;
                        homeOwerUser.TenantId = tenantId;
                        homeOwer.ValidateCode = string.Empty;
                        await _homeOwerUserManager.UpdateAsync(homeOwerUser);
                        await _homeOwerManager.UpdateAsync(homeOwer);

                        return Ok(new { HomeOwer = AutoMapper.Mapper.Map<HomeOwerDto>(homeOwer), Community = AutoMapper.Mapper.Map<CommunityDto>(homeOwer.Community) });
                    }
                    else
                    {
                        throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.ValidateCodeError);
                    }
                }
            }
        }
    }
}
