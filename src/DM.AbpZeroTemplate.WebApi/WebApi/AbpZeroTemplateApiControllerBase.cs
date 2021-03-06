using Abp.UI;
using Abp.WebApi.Controllers;
using DM.AbpZeroTemplate.DoorSystem;
using System.Web;
using System.Web.Http;

namespace DM.AbpZeroTemplate.WebApi
{
    public abstract class AbpZeroTemplateApiControllerBase : AbpApiController
    {
        protected readonly HomeOwerUserManager _homeOwerUserManager;

        protected HomeOwerUser User;

        protected AbpZeroTemplateApiControllerBase()
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }

        protected AbpZeroTemplateApiControllerBase(HomeOwerUserManager homeOwerUserManager)
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
            _homeOwerUserManager = homeOwerUserManager;
        }

        protected void AuthUser()
        {
            var userName = HttpContext.Current.Request["userName"];
            var token = HttpContext.Current.Request["token"];
            if (!_homeOwerUserManager.AuthUser(userName, token))
            {
                throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.UserAuthError);
            }
            else
            {
                User = _homeOwerUserManager.HomeOwerUserRepository.FirstOrDefault(u => u.UserName == userName);
            }
        }
    }
}