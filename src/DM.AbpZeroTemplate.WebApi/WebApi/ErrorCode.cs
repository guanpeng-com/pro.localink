using Abp.Dependency;
using Abp.Localization;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.WebApi
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum ErrorCodeType
    {
        /// <summary>
        /// 位置错误
        /// </summary>
        UnknowError = 1,
        /// <summary>
        /// 用户验证失败
        /// </summary>
        UserAuthError = 2,
        /// <summary>
        /// 业主不存在
        /// </summary>
        HomeOwerNotExists = 3,
        /// <summary>
        /// 业主未认证
        /// </summary>
        HomeOwerUserNotExists = 4,
        /// <summary>
        /// 验证码发送失败
        /// </summary>
        SMSSendCodeError = 5,
        /// <summary>
        /// 验证码错误
        /// </summary>
        ValidateCodeError = 6,
        /// <summary>
        /// 快递已经领取
        /// </summary>
        DeliveryIsGathered = 7,
        /// <summary>
        /// 业主没有此类型的钥匙
        /// </summary>
        HomeOwerDoorNotExists = 8,
        /// <summary>
        /// 业主已认证
        /// </summary>
        HomeOwerUserIsExists = 9,
        /// <summary>
        /// 业主正在审核中，请耐心等待
        /// </summary>
        HomeOwerUserIsAuthing = 10

    }

    /// <summary>
    /// 错误代码辅助类
    /// </summary>
    public class ErrorCodeTypeUtils
    {
        /// <summary>
        /// 抛出对应类型的错误
        /// </summary>
        /// <param name="type"></param>
        /// <param name="details"></param>
        public static Exception ThrowError(ErrorCodeType type, string details = null)
        {
            if (type == ErrorCodeType.UserAuthError)
            {
                return new UserFriendlyException((int)ErrorCodeType.UserAuthError, L(ErrorCodeType.UserAuthError.ToString()), details);
            }
            else if (type == ErrorCodeType.HomeOwerNotExists)
            {
                return new UserFriendlyException((int)ErrorCodeType.HomeOwerNotExists, L(ErrorCodeType.HomeOwerNotExists.ToString()), details);
            }
            else if (type == ErrorCodeType.HomeOwerUserNotExists)
            {
                return new UserFriendlyException((int)ErrorCodeType.HomeOwerUserNotExists, L(ErrorCodeType.HomeOwerUserNotExists.ToString()), details);
            }
            else if (type == ErrorCodeType.SMSSendCodeError)
            {
                return new UserFriendlyException((int)ErrorCodeType.SMSSendCodeError, L(ErrorCodeType.SMSSendCodeError.ToString()), details);
            }
            else if (type == ErrorCodeType.ValidateCodeError)
            {
                return new UserFriendlyException((int)ErrorCodeType.ValidateCodeError, L(ErrorCodeType.ValidateCodeError.ToString()), details);
            }
            else if (type == ErrorCodeType.DeliveryIsGathered)
            {
                return new UserFriendlyException((int)ErrorCodeType.DeliveryIsGathered, L(ErrorCodeType.DeliveryIsGathered.ToString()), details);
            }
            else if (type == ErrorCodeType.HomeOwerDoorNotExists)
            {
                return new UserFriendlyException((int)ErrorCodeType.HomeOwerDoorNotExists, L(ErrorCodeType.HomeOwerDoorNotExists.ToString()), details);
            }
            else if (type == ErrorCodeType.HomeOwerUserIsExists)
            {
                return new UserFriendlyException((int)ErrorCodeType.HomeOwerUserIsExists, L(ErrorCodeType.HomeOwerUserIsExists.ToString()), details);
            }
            else if (type == ErrorCodeType.HomeOwerUserIsAuthing)
            {
                return new UserFriendlyException((int)ErrorCodeType.HomeOwerUserIsAuthing, L(ErrorCodeType.HomeOwerUserIsAuthing.ToString()), details);
            }
            else
            {
                return new UserFriendlyException((int)ErrorCodeType.UnknowError, L(ErrorCodeType.UnknowError.ToString()), details);
            }
        }

        /// <summary>
        /// 本地化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string L(string name)
        {
            ILocalizationManager localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
            return localizationManager.GetString(AbpZeroTemplateConsts.LocalizationSourceName, name);
        }
    }
}
