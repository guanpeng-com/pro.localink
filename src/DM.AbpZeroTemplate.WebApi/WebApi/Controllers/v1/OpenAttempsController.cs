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
    [RoutePrefix("api/v1/OpenAttemps")]
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
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">用户令牌</param>
        /// <param name="homeOwerId">业主Id</param>
        /// <param name="isSuccess">是否成功</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> CreateOpenAttemp(string userName, long homeOwerId, bool isSuccess, string token, int? tenantId = null)
        {
            if (!base.AuthUser()) return Unauthorized();
            try
            {
                using (CurrentUnitOfWork.SetTenantId(tenantId))
                {
                    var openAttemp = new OpenAttemp();

                    openAttemp.HomeOwerId = homeOwerId;
                    openAttemp.UserName = userName;
                    openAttemp.IsSuccess = isSuccess;

                    await _openAttempManager.CreateAsync(openAttemp);

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
