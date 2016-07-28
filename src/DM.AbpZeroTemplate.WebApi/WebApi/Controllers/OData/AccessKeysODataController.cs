using Abp.Domain.Repositories;
using DM.AbpZeroTemplate.DoorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.WebApi.Controllers
{
    public class AccessKeysODataController : ODataBaseController<AccessKey>
    {
        public AccessKeysODataController(IRepository<AccessKey, long> repository)
            : base(repository)
        {

        }
    }
}
