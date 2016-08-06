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
using System.Collections;
using Abp.Timing;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.Web.Models;
using Abp.UI;

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/Deliverys")]
    [WrapResult]
    public class DeliverysController : AbpZeroTemplateApiControllerBase
    {
        private readonly DeliveryManager _deliveryManager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly TenantManager _tenantManager;

        private readonly DeliveryService _deliveryService;

        public DeliverysController(
            DeliveryManager deliveryManager,
            TenantManager tenantManager,
            HomeOwerUserManager homeOwerUserManager,
            DeliveryService deliveryService,
            HomeOwerManager homeOwerManager)
            : base(homeOwerUserManager)
        {
            _deliveryManager = deliveryManager;
            _tenantManager = tenantManager;
            _deliveryService = deliveryService;
            _homeOwerManager = homeOwerManager;
        }

        /// <summary>
        /// 获取业主的快递集合（分页）
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="skipCount">从第 skipCount+1 条开始</param>
        /// <param name="maxResultCount">每页条数</param>
        /// <param name="sorting">排序字段，可传null</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> GetDeliverys(string userName, string token, int skipCount, int maxResultCount, string sorting = null, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);

                var input = new GetDeliverysInput();
                input.HomeOwerId = homeOwerUser.HomeOwerId;
                input.MaxResultCount = maxResultCount;
                input.SkipCount = skipCount;
                if (!string.IsNullOrEmpty(sorting))
                    input.Sorting = sorting;

                return Ok(await _deliveryService.GetAllDeliverys(input));
            }
        }

        /// <summary>
        /// 业主领取快递
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="homeOwerId">业主Id</param>
        /// <param name="deliveryId">快递Id</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> GatherDelivery(string userName, string token, long homeOwerId, long deliveryId, string code, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(homeOwerId);
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                var delivery = await _deliveryManager.DeliveryRepository.FirstOrDefaultAsync(deliveryId);
                if (delivery.IsGather)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.DeliveryIsGathered);
                }
                if (delivery.HomeOwerId != homeOwerId)
                {
                    return Ok(new { IsReplaceGather = true });
                }
                else if (!string.IsNullOrEmpty(code) && code == delivery.Token)
                {
                    delivery.IsGather = true;
                    delivery.GatherTime = Clock.Now;
                    delivery.Token = string.Empty;
                    await _deliveryManager.UpdateAsync(delivery);
                    return Ok(new { IsReplaceGather = false });
                }
                else
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.ValidateCodeError);
                }
            }
        }

        /// <summary>
        /// 业主代领快递
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="homeOwerId">业主Id</param>
        /// <param name="deliveryId">快递Id</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> ReplaceGatherDelivery(string userName, string token, long homeOwerId, long deliveryId, string code, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(homeOwerId);
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                var delivery = await _deliveryManager.DeliveryRepository.FirstOrDefaultAsync(deliveryId);
                if (delivery.IsGather)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.DeliveryIsGathered);
                }
                if (delivery.HomeOwerId != homeOwerId)
                {
                    if (delivery.Token == code)
                    {
                        delivery.IsGather = true;
                        delivery.GatherTime = Clock.Now;
                        delivery.IsReplace = true;
                        delivery.ReplaceHomeOwerId = homeOwerId;
                        delivery.Token = string.Empty;
                        await _deliveryManager.UpdateAsync(delivery);
                        return Ok();
                    }
                    else
                    {
                        throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.ValidateCodeError);
                    }
                }
                throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.UnknowError);
            }
        }
    }
}
