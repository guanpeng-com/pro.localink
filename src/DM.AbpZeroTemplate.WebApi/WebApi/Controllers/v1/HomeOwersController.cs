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

        public HomeOwersController(
            CommunityManager communityManager,
            AccessKeyManager accessKeyManager,
            HomeOwerManager homeOwerManager,
            DoorManager doorManager,
            TenantManager tenantManager)
        {
            _communityManager = communityManager;
            _accessKeyManager = accessKeyManager;
            _homeOwerManager = homeOwerManager;
            _tenantManager = tenantManager;
            _doorManager = doorManager;
        }

        /// <summary>
        ///  获取业主的钥匙
        /// </summary>
        /// <param name="tenancyName">公司名称</param>
        /// <param name="id">业主Id</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        [Route("/{id:long}/AccessKeys")]
        public async virtual Task<IHttpActionResult> GetAccessKeys(string tenancyName, long id)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(tenancyName);
            int? tenantId = tenant == null ? (int?)null : tenant.Id;
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwer = _homeOwerManager.HomeOwerRepository.FirstOrDefault(h => h.Id == id);
                if (homeOwer == null)
                {
                    return NotFound();
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
    }
}
