using Abp.Runtime.Validation;

namespace DM.AbpZeroTemplate.Configuration.Tenants.Dto
{
    public class TenantUserManagementSettingsEditDto : IValidate
    {
        public bool AllowSelfRegistration { get; set; }

        public bool IsNewRegisteredUserActiveByDefault { get; set; }

        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        public bool UseCaptchaOnRegistration { get; set; }


        public string PhoneCountryCode { get; set; }


        public long RootAreaId { get; set; }
    }
}