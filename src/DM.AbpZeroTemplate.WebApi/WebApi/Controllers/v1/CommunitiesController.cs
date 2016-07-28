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

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/Communities")]
    public class CommunitiesController : AbpZeroTemplateApiControllerBase
    {
        private readonly CommunityManager _communityManager;
        private readonly TenantManager _tenantManager;

        public CommunitiesController(
            CommunityManager communityManager,
            TenantManager tenantManager,
            HomeOwerUserManager homeOwerUserManager)
            : base(homeOwerUserManager)
        {
            _communityManager = communityManager;
            _tenantManager = tenantManager;
        }

        /// <summary>
        /// 获取全部小区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> GetAllCommunities(string userName, string token)
        {
            if (!base.AuthUser()) return Unauthorized();
            try
            {
                Dictionary<string, ArrayList> dic = new Dictionary<string, ArrayList>();
                var tenants = _tenantManager.Tenants.ToList();
                foreach (var t in tenants)
                {
                    int? tenantId = t == null ? (int?)null : t.Id;
                    if (t.TenancyName == Tenant.DefaultTenantName)
                    {
                        tenantId = null;
                    }
                    using (CurrentUnitOfWork.SetTenantId(tenantId))
                    {
                        var communities = await _communityManager.CommunityRepository.GetAllListAsync(c => c.IsAuth == true);
                        communities.ForEach(c =>
                        {
                            if (!dic.Keys.Contains(c.Address))
                            {
                                dic[c.Address] = new ArrayList();
                                dic[c.Address].Add(new { c.Id, c.Name, c.Address, c.TenantId });
                            }
                            else
                            {
                                dic[c.Address].Add(new { c.Id, c.Name, c.Address, c.TenantId });
                            }
                        });
                    }
                };
                return Ok(dic);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
    }
}
