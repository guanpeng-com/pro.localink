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
    /// 业主类型枚举
    /// </summary>
    public enum EHomeOwerGroupType : byte
    {
        /// <summary>
        /// 管理员
        /// </summary>
        ManagingAgent = 1,
        /// <summary>
        /// 房屋所有者，但是不住
        /// </summary>
        Owner = 2,
        /// <summary>
        /// 房屋所有者，住
        /// </summary>
        OwnerOccupier = 3,
        /// <summary>
        /// 租户
        /// </summary>
        Tenant = 4
    }

    public class EHomeOwerGroupTypeUtils
    {
        public static List<NameValueDto> GetItemCollection()
        {
            List<NameValueDto> list = new List<NameValueDto>();
            list.Add(new NameValueDto(EHomeOwerGroupType.ManagingAgent.ToString(), EHomeOwerGroupType.ManagingAgent.ToString()));
            list.Add(new NameValueDto(EHomeOwerGroupType.Owner.ToString(), EHomeOwerGroupType.Owner.ToString()));
            list.Add(new NameValueDto(EHomeOwerGroupType.OwnerOccupier.ToString(), EHomeOwerGroupType.OwnerOccupier.ToString()));
            return list;
        }

        public static EHomeOwerGroupType GetValue(string status)
        {
            if (status == "ReportSend")
            {
                return EHomeOwerGroupType.ManagingAgent;
            }
            else if (status == "ReportProcessing")
            {
                return EHomeOwerGroupType.Owner;
            }
            else if (status == "ReportFinished")
            {
                return EHomeOwerGroupType.OwnerOccupier;
            }
            else
            {
                throw new UserFriendlyException("Report Status Exception");
            }
        }
    }
}
