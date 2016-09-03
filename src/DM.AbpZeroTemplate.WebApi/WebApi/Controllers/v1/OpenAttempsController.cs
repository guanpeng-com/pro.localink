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
using Abp.Web.Models;
using DM.AbpZeroTemplate.WebApi.Models;

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/OpenAttemps")]
    [WrapResult]
    public class OpenAttempsController : AbpZeroTemplateApiControllerBase
    {
        private readonly OpenAttempManager _openAttempManager;
        private readonly TenantManager _tenantManager;

        public OpenAttempsController(
            OpenAttempManager openAttempManager,
            TenantManager tenantManager,
            HomeOwerUserManager homeOwerUserManager)
            : base(homeOwerUserManager)
        {
            _openAttempManager = openAttempManager;
            _tenantManager = tenantManager;
        }

        /// <summary>
        ///  添加开锁记录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="createOpenAttempModel">post参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> CreateOpenAttemp(string userName, string token, [FromBody]CreateOpenAttempModel createOpenAttempModel)
        {
            var tenantId = createOpenAttempModel.TenantId;
            var communityId = createOpenAttempModel.CommunityId;
            var homeOwerId = createOpenAttempModel.HomeOwerId;
            var isSuccess = createOpenAttempModel.IsSuccess;
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var openAttemp = new OpenAttemp(tenantId, communityId);

                openAttemp.HomeOwerId = homeOwerId;
                openAttemp.UserName = userName;
                openAttemp.IsSuccess = isSuccess;

                await _openAttempManager.CreateAsync(openAttemp);

                return Ok();
            }

        }
    }
}
