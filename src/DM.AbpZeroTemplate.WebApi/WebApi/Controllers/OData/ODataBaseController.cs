using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.WebApi.OData.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DM.AbpZeroTemplate.WebApi.Controllers
{
    public class ODataBaseController<T> : AbpODataEntityController<T, long>
        where T : class, IEntity<long>
    {
        public ODataBaseController(IRepository<T, long> repository)
            : base(repository)
        {
            Auth();
        }

        private void Auth()
        {
            var userName = HttpContext.Current.Request["UserName"];
            var token = HttpContext.Current.Request["Token"];
            if (userName != "admin" || token != "admin888")
            {
                //throw new UserFriendlyException("Auth Error", "User has no access");
                HttpContext.Current.Response.Write("User has no access");
                HttpContext.Current.Response.End();
                return;
            }
        }
    }
}
