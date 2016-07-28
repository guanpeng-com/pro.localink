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

namespace DM.AbpZeroTemplate.WebApi.Controllers.v2
{
    [VersionedRoute("api/version", 2)]
    [RoutePrefix("api/v2/HomeOwersV2")]
    public class HomeOwersV2Controller : AbpZeroTemplateApiControllerBase
    {
        [Route("/{id:long}/AccessKeys")]
        [HttpGet]
        public void GetAccessKeys(long id)
        {
            HttpContext.Current.Response.Write("GetAccessKeysV2 is testing");
            HttpContext.Current.Response.End();
        }

        [Route("/{id:long}/Test")]
        [HttpGet]
        public void Test(long id)
        {
            HttpContext.Current.Response.Write("v2 is testing");
            HttpContext.Current.Response.End();
        }
    }
}
