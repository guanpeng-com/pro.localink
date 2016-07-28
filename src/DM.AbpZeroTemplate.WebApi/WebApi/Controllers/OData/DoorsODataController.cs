using Abp.Domain.Repositories;
using DM.AbpZeroTemplate.DoorSystem;
using DM.AbpZeroTemplate.DoorSystem.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.WebApi.Controllers
{
    public class DoorsODataController : ODataBaseController<Door>
    {
        public DoorsODataController(IRepository<Door, long> repository)
            : base(repository)
        {
        }
    }
}
