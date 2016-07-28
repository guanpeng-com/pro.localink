using Abp.WebApi.Controllers;
using DM.AbpZeroTemplate.DoorSystem;
using System.Web;
using System.Web.Http;

namespace DM.AbpZeroTemplate.WebApi
{
    public abstract class AbpZeroTemplateApiControllerBase : AbpApiController
    {
        private readonly HomeOwerUserManager _homeOwerUserManager;

        protected AbpZeroTemplateApiControllerBase()
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }

        protected AbpZeroTemplateApiControllerBase(HomeOwerUserManager homeOwerUserManager)
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
            _homeOwerUserManager = homeOwerUserManager;
        }

        protected bool AuthUser()
        {
            var userName = HttpContext.Current.Request["userName"];
            var token = HttpContext.Current.Request["token"];
            return  _homeOwerUserManager.AuthUser(userName, token);
        }
    }
}