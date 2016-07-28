using Abp.Domain.Repositories;
using DM.AbpZeroTemplate.DoorSystem;
using DM.AbpZeroTemplate.DoorSystem.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace DM.AbpZeroTemplate.WebApi.Controllers
{
    public class HomeOwersODataController : ODataBaseController<HomeOwer>
    {
        private readonly CommunityManager _communityManager;
        private readonly AccessKeyManager _accessKeyManager;
        private readonly HomeOwerManager _homeOwerManager;

        public HomeOwersODataController(IRepository<HomeOwer, long> repository,
            CommunityManager communityManager,
            AccessKeyManager accessKeyManager,
            HomeOwerManager homeOwerManager)
            : base(repository)
        {
            _communityManager = communityManager;
            _accessKeyManager = accessKeyManager;
            _homeOwerManager = homeOwerManager;
        }

        //[EnableQuery]
        //[ODataRoute("/HomeOwers({key})/AccessKeys")]
        //public async Task<IList<AccessKey>> GetAccessKeys([FromODataUri] long key)
        //{
        //    var tenantStr = HttpContext.Current.Request.QueryString["tenantId"];
        //    int? tenantId = tenantStr == null ? (int?)null : Convert.ToInt32(tenantStr);

        //    using (UnitOfWorkManager.Current.SetTenantId(tenantId))
        //    {
        //        var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(h => h.Id == key);

        //        var query = await _accessKeyManager.AccessKeyRepository.GetAllListAsync(a => a.HomeOwerId == homeOwer.Id);
        //        return query;
        //    }
        //}
    }
}
