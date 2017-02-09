using Abp.Application.Services.Dto;
using Abp.CMS;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Localization;
using Abp.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroDoor.DoorSystem.Enums
{
    /// <summary>
    /// 消息状态枚举
    /// </summary>
    public enum EMessageStatusType : byte
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 1,
        /// <summary>
        /// 已发送
        /// </summary>
        Sent = 2,
        /// <summary>
        /// 撤回（未启用）
        /// </summary>
        Cancel = 3
    }

    public class EMessageStatusTypeUtils
    {
        public static List<NameValueDto> GetItemCollection()
        {
            List<NameValueDto> list = new List<NameValueDto>();
            list.Add(new NameValueDto(EMessageStatusType.Draft.ToString(), EMessageStatusType.Draft.ToString()));
            list.Add(new NameValueDto(EMessageStatusType.Sent.ToString(), EMessageStatusType.Sent.ToString()));

            return list;
        }

        public static EMessageStatusType GetValue(string status)
        {
            if (status == "Draft")
            {
                return EMessageStatusType.Draft;
            }
            else if (status == "Sent")
            {
                return EMessageStatusType.Sent;
            }
            else if (status == "Cancel")
            {
                return EMessageStatusType.Cancel;
            }
            else
            {
                throw new UserFriendlyException("Message Status Exception");
            }
        }
    }
}
