using Abp.Domain.Repositories;
using Abp.WebApi.Controllers;
using DM.AbpZeroTemplate.DoorSystem.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DM.AbpZeroTemplate.WebApi.Controllers
{
    public class CommunitysODataController : ODataBaseController<Community>
    {
        public CommunitysODataController(IRepository<Community, long> repository)
            : base(repository)
        {
        }
    }
}
