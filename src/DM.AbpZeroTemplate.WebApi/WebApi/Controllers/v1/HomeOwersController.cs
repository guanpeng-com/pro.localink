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
using DM.AbpZeroDoor.DoorSystem.Enums;
using DM.AbpZeroTemplate.WebApi.Models;
using System.Collections;

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
                if (homeOwer.Status == EHomeOwerStatusType.Waiting)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserIsAuthing);
                }
                var list = (from a in _accessKeyManager.AccessKeyRepository.GetAll()
                            join d in _doorManager.DoorRepository.GetAll() on a.Door equals d
                            where a.HomeOwer.Id == homeOwer.Id && d.IsAuth
                            select new { KeyId = a.LockId, KeyValidity = a.Validity, CommunityId = d.DepartId, KeyName = d.Name, KeyType = d.DoorType, IsAuth = a.IsAuth }
                            ).ToList();

                var result = new ArrayList();
                list.ForEach(i =>
                {
                    result.Add(new
                    {
                        i.KeyId,
                        i.KeyValidity,
                        i.CommunityId,
                        i.KeyName,
                        KeyType = EDoorTypeUtils.GetEnum(i.KeyType),
                        i.IsAuth
                    });
                });

                return Ok(new
                {
                    AppKey = _doorSystemSdk.Params["app_key"],
                    UserId = homeOwer.Phone,
                    AccessKeys = result
                });
            }
        }

        /// <summary>
        ///  刷新业主的钥匙
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="id">业主Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        [Route("/{id:long}/FreshAccessKeys")]
        public virtual async Task<IHttpActionResult> FreshAccessKeys(long id, string userName, string token, int? tenantId = null)
        {
            base.AuthUser();
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
                if (homeOwer.Status == EHomeOwerStatusType.Waiting)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserIsAuthing);
                }
                //获取业主门禁，判断是否已经添加钥匙，是否已经认证
                foreach (var door in homeOwer.Doors)
                {
                    var key = await _accessKeyManager.AccessKeyRepository.FirstOrDefaultAsync(k => k.Door == door && k.HomeOwer.Id == homeOwer.Id);
                    if (key == null)
                    {
                        key = new AccessKey(CurrentUnitOfWork.GetTenantId(), door, homeOwer, DateTime.Now.AddYears(50), homeOwer.CommunityId);

                        try
                        {
                            await _accessKeyManager.CreateAsync(key);

                        }
                        catch (UserFriendlyException ex)
                        {
                            if (ex.Message == "10")
                            {
                                ErrorCodeTypeUtils.ThrowError(ErrorCodeType.CreatedAccessKeyIsExistsButIsNotAuth);
                            }
                            else if (ex.Message == "11")
                            {
                                ErrorCodeTypeUtils.ThrowError(ErrorCodeType.CreatedAccessKeyIsExists);
                            }
                        }
                        key.GetKey(door.PId, homeOwer.Phone, key.Validity);
                    }
                    else if (!key.IsAuth)
                    {
                        key.GetKey(door.PId, homeOwer.Phone, key.Validity);
                    }
                }

                var list = (from a in _accessKeyManager.AccessKeyRepository.GetAll()
                            join d in _doorManager.DoorRepository.GetAll() on a.Door equals d
                            where a.HomeOwer.Id == homeOwer.Id && d.IsAuth
                            select new { KeyId = a.LockId, KeyValidity = a.Validity, CommunityId = d.DepartId, KeyName = d.Name, KeyType = d.DoorType, IsAuth = a.IsAuth }
                            ).ToList();

                var result = new ArrayList();
                list.ForEach(i =>
                {
                    result.Add(new
                    {
                        i.KeyId,
                        i.KeyValidity,
                        i.CommunityId,
                        i.KeyName,
                        KeyType = EDoorTypeUtils.GetEnum(i.KeyType),
                        i.IsAuth
                    });
                });

                return Ok(new
                {
                    AppKey = _doorSystemSdk.Params["app_key"],
                    UserId = homeOwer.Phone,
                    AccessKeys = result
                });
            }
        }

        /// <summary>
        /// 根据钥匙类型申请钥匙 EDoorType doorType, DateTime vilidity, 
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="applyAccessKeyModel">post参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [Route("/ApplyAccessKey")]
        public virtual async Task<IHttpActionResult> ApplyAccessKey(string userName, string token, [FromBody]ApplyAccessKeyModel applyAccessKeyModel)
        {
            var tenantId = applyAccessKeyModel.TenantId;
            var homeOwerId = applyAccessKeyModel.HomeOwerId;
            var communityId = applyAccessKeyModel.CommunityId;
            var doorType = applyAccessKeyModel.DoorType;
            var vilidity = applyAccessKeyModel.Vilidity;
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var doors = from d in _doorManager.DoorRepository.GetAll()
                            where d.DoorType == doorType.ToString() && d.IsAuth
                            select d;
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(homeOwerId);
                if (homeOwer.Doors.Count == 0)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerDoorNotExists);
                }
                else
                {
                    foreach (var door in homeOwer.Doors)
                    {
                        var accessKey = new AccessKey(tenantId, door, homeOwer, vilidity, communityId);
                        await _accessKeyManager.CreateAsync(accessKey);
                    }
                    return Ok();
                }
            }
        }

        /// <summary>
        /// 删除用户钥匙
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="applyAccessKeyModel">post参数</param>
        /// <returns></returns>
        [HttpDelete]
        [UnitOfWork]
        [Route("/DeleteAccessKey")]
        public virtual async Task<IHttpActionResult> DeleteAccessKey(string userName, string token, [FromBody]ApplyAccessKeyModel applyAccessKeyModel)
        {
            var tenantId = applyAccessKeyModel.TenantId;
            var homeOwerId = applyAccessKeyModel.HomeOwerId;
            var communityId = applyAccessKeyModel.CommunityId;
            var doorType = applyAccessKeyModel.DoorType;
            var vilidity = applyAccessKeyModel.Vilidity;
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var doors = from d in _doorManager.DoorRepository.GetAll()
                            where d.DoorType == doorType.ToString() && d.IsAuth
                            select d;
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(homeOwerId);

                foreach (var door in homeOwer.Doors)
                {
                    await _accessKeyManager.AccessKeyRepository.DeleteAsync(a => a.Door == door);
                }
                return Ok();
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
            var homeOwerUser = await _homeOwerUserManager.HomeOwerUserRepository.FirstOrDefaultAsync(h => h.UserName == userName);
            if (homeOwerUser == null)
            {
                homeOwerUser = new HomeOwerUser(userName, token);
                await _homeOwerUserManager.CreateAsync(homeOwerUser);
            }
            else
            {
                homeOwerUser.Token = token;
                await _homeOwerUserManager.UpdateAsync(homeOwerUser);
            }
            return Ok();
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="from">发送方名称</param>
        /// <param name="countryCode">国家代码（比如，中国-86）</param>
        /// <param name="to">接受手机号</param>
        /// <returns></returns>
        [SecretVersionedRoute]
        [HttpPost]
        [UnitOfWork]
        [Route("/SendValidateCode")]
        public virtual IHttpActionResult SendValidateCode(string from, string countryCode, string to)
        {
            SMSClient smsClient = new SMSClient();
            var response = smsClient.SendVerify(from, countryCode + to);
            if (response.Status == "0")
            {
                return Ok(new { requestId = response.RequestId });
            }
            else
            {
                throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.SMSSendCodeError, response.ErrorText);
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="requestId">requestId</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [SecretVersionedRoute]
        [HttpPost]
        [UnitOfWork]
        [Route("/ValidateCode")]
        public virtual IHttpActionResult ValidateCode(string requestId, string code)
        {
            SMSClient smsClient = new SMSClient();
            var response = smsClient.Verify(requestId, code);
            if (response.Status == "0")
            {
                return Ok();
            }
            else
            {
                throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.SMSSendCodeError, response.ErrorText);
            }
        }

        /// <summary>
        /// 认证用户为业主，第一步，发送验证码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="authUserSendCodeModel">post参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [Route("/AuthUserSendCode")]
        public async virtual Task<IHttpActionResult> AuthUserSendCode(string userName, string token, [FromBody]AuthUserSendCodeModel authUserSendCodeModel)
        {
            var tenantId = authUserSendCodeModel.TenantId;
            var communityId = authUserSendCodeModel.CommunityId;
            var phone = authUserSendCodeModel.Phone;
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);
                var homeOwer = await _homeOwerManager.GetHomeOwerByNameAndPhoneAndCommunityId(communityId, phone);
                //if (homeOwerUser == null)
                //{
                //    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserNotExists);
                //}
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                if (homeOwer.Status == EHomeOwerStatusType.Done)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserIsExists);
                }
                else
                {
                    //发送验证码
                    SMSClient smsClient = new SMSClient();
                    var phoneCountryCode = await SettingManager.GetSettingValueAsync(AppSettings.UserManagement.PhoneCountryCode);
                    var response = smsClient.SendVerify("Localink", phoneCountryCode + homeOwer.Phone);
                    if (response.Status == "0")
                    {
                        homeOwer.ValidateCode = response.RequestId;
                        await _homeOwerManager.UpdateAsync(homeOwer);
                        return Ok();
                    }
                    else
                    {
                        throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.SMSSendCodeError, response.ErrorText);
                    }
                }
            }
        }

        /// <summary>
        /// 认证用户为业主，第二步，验证验证码，完成验证
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="authUserValidateCodeModel">post参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [Route("/AuthUserValidateCode")]
        public async virtual Task<IHttpActionResult> AuthUserValidateCode(string userName, string token, [FromBody]AuthUserValidateCodeModel authUserValidateCodeModel)
        {
            var tenantId = authUserValidateCodeModel.TenantId;
            var phone = authUserValidateCodeModel.Phone;
            var communityId = authUserValidateCodeModel.CommunityId;
            var code = authUserValidateCodeModel.Code;
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);
                var homeOwer = await _homeOwerManager.GetHomeOwerByNameAndPhoneAndCommunityId(communityId, phone);
                if (homeOwerUser == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserNotExists);
                }
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                else if (homeOwer.Status == EHomeOwerStatusType.Done)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerUserIsExists);
                }
                else
                {
                    //验证验证码是否正确
                    SMSClient smsClient = new SMSClient();
                    var response = smsClient.Verify(homeOwer.ValidateCode, code);
                    if (response.Status == "0")
                    {
                        homeOwerUser.HomeOwerId = homeOwer.Id;
                        homeOwerUser.CommunityId = homeOwer.CommunityId;
                        homeOwerUser.TenantId = tenantId;
                        homeOwer.ValidateCode = string.Empty;
                        homeOwer.Status = EHomeOwerStatusType.Waiting;
                        await _homeOwerUserManager.UpdateAsync(homeOwerUser);
                        await _homeOwerManager.UpdateAsync(homeOwer);

                        return Ok(new { HomeOwer = AutoMapper.Mapper.Map<HomeOwerDto>(homeOwer), Community = AutoMapper.Mapper.Map<CommunityDto>(homeOwer.FlatNumbers.First().Building.Community) });
                    }
                    else
                    {
                        throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.ValidateCodeError);
                    }
                }
            }
        }

        /// <summary>
        /// 保存用户绑定的小区ID
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="saveUserCommunityIdModel">post参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [Route("/SaveUserCommunityId")]
        public async virtual Task<IHttpActionResult> SaveUserCommunityId(string userName, string token, [FromBody]SaveUserCommunityIdModel saveUserCommunityIdModel)
        {
            var tenantId = saveUserCommunityIdModel.TenantId;
            var communityId = saveUserCommunityIdModel.CommunityId;
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                base.User.CommunityId = communityId;
                base.User.TenantId = tenantId;
                await _homeOwerUserManager.UpdateAsync(base.User);
                return Ok();
            }
        }

        /// <summary>
        /// 登录之后，获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        [Route("/GetUserInfo")]
        public virtual IHttpActionResult GetUserInfo(string userName, string token)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(base.User.TenantId))
            {
                var communityId = base.User.CommunityId.HasValue ? base.User.CommunityId.Value : 0;
                var community = _communityManager.CommunityRepository.FirstOrDefault(c => c.Id == communityId);
                var homeOwerId = base.User.HomeOwerId.HasValue ? base.User.HomeOwerId : 0;
                var homeOwer = _homeOwerManager.HomeOwerRepository.FirstOrDefault(h => h.Id == homeOwerId && h.CommunityId == communityId);
                return Ok(new
                {
                    base.User.UserName,
                    base.User.TenantId,
                    base.User.CommunityId,
                    IsAuth = homeOwer != null ? homeOwer.Status == EHomeOwerStatusType.Done : false,
                    base.User.HomeOwerId,
                    CommunityName = community != null ? community.Name : string.Empty,
                    CommunityAddress = community != null ? community.Address : string.Empty,
                    communityImages = community != null ? community.Images : string.Empty
                });
            }

        }
    }
}
