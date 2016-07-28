using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Threading;
using DM.AbpZeroTemplate.Configuration;
using DM.AbpZeroTemplate.Sessions;
using DM.AbpZeroTemplate.Web.Models.Layout;
using DM.AbpZeroTemplate.Web.Navigation;
using DM.AbpZeroTemplate.MultiTenancy;

namespace DM.AbpZeroTemplate.Web.Controllers
{
    /// <summary>
    /// Layout for 'front end' pages.
    /// </summary>
    public class LayoutController : AbpZeroTemplateControllerBase
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly TenantManager _tenantManager;

        public LayoutController(ISessionAppService sessionAppService, IUserNavigationManager userNavigationManager, IMultiTenancyConfig multiTenancyConfig, TenantManager tenantManager)
        {
            _sessionAppService = sessionAppService;
            _userNavigationManager = userNavigationManager;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantManager = tenantManager;
        }

        [ChildActionOnly]
        public PartialViewResult Header(string currentPageName = "")
        {
            var headerModel = new HeaderViewModel();

            if (!AbpSession.TenantId.HasValue)
            {
                HeaderProcess(currentPageName, headerModel);
            }
            else
            {
                using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId.Value))
                {
                    HeaderProcess(currentPageName, headerModel);
                }
            }



            return PartialView("~/Views/Layout/_Header.cshtml", headerModel);
        }

        private void HeaderProcess(string currentPageName, HeaderViewModel headerModel)
        {
            if (AbpSession.UserId.HasValue)
            {
                headerModel.LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations());
            }

            headerModel.Languages = LocalizationManager.GetAllLanguages();
            headerModel.CurrentLanguage = LocalizationManager.CurrentLanguage;

            if (AbpSession.UserId.HasValue)
            {
                headerModel.Menu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync(FrontEndNavigationProvider.MenuName, new Abp.UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value)));
            }
            else
            {
                headerModel.Menu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync(FrontEndNavigationProvider.MenuName, AbpSession.UserId, AbpSession.TenantId));
                headerModel.CurrentPageName = currentPageName;
            }
            headerModel.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            headerModel.TenantRegistrationEnabled = SettingManager.GetSettingValue<bool>(AppSettings.TenantManagement.AllowSelfRegistration);
        }
    }
}