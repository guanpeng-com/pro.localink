using Abp.Application.Services.Dto;
using Abp.Apps;
using Abp.CMS;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.UI;
using DM.AbpZeroTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroDoor.DoorSystem.Enums
{
    /// <summary>
    /// 上传文件类型枚举
    /// </summary>
    public enum EFileUploadType : byte
    {
        /// <summary>
        /// 普通文件，存放在 /Upload/Common 文件夹中
        /// </summary>
        Common = 1,
        /// <summary>
        /// 临时文件，存放在 /Upload/Temp 文件夹中
        /// </summary>
        Temp = 2,
        /// <summary>
        /// 应用普通文件，存放在 <AppPath>/Upload/Common 文件夹中
        /// </summary>
        AppCommon = 3,
        /// <summary>
        /// 应用普通文件，存放在 <AppPath>/Upload/Temp 文件夹中
        /// </summary>
        AppTemp = 4
    }

    public class EFileUploadTypeUtils
    {
        public static string GetFileUploadPath(string type, IAppFolders appFolders, App appInfo = null)
        {
            if (type == "Common")
            {
                return appFolders.CommonFileFolder;
            }
            else if (type == "Temp")
            {
                return appFolders.TempFileFolder;
            }
            else if (type == "AppCommon")
            {
                if (appInfo != null)
                    return appFolders.AppCommonFileFolder.Replace("APP_PATH", appInfo.AppDir);
                else
                    throw new UserFriendlyException("appInfo is null. It must not be null.");
            }
            else if (type == "AppTemp")
            {
                if (appInfo != null)
                    return appFolders.AppTempFileFolder.Replace("APP_PATH", appInfo.AppDir);
                else
                    throw new UserFriendlyException("appInfo is null. It must not be null.");
            }
            else
            {
                throw new UserFriendlyException("Report Status Exception");
            }
        }
    }
}
