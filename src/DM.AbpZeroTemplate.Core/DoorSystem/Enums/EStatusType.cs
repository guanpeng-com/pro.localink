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
    /// 业主状态枚举
    /// </summary>
    public enum EHomeOwerStatusType : byte
    {
        /// <summary>
        /// 未认证
        /// </summary>
        Initial = 1,
        /// <summary>
        /// 等待中
        /// </summary>
        Waiting = 2,
        /// <summary>
        /// 已认证
        /// </summary>
        Done = 3
    }

    public class EHomeOwerStatusTypeUtils
    {
        public static List<NameValueDto> GetItemCollection()
        {
            List<NameValueDto> list = new List<NameValueDto>();
            list.Add(new NameValueDto(EHomeOwerStatusType.Initial.ToString(), EHomeOwerStatusType.Initial.ToString()));
            list.Add(new NameValueDto(EHomeOwerStatusType.Waiting.ToString(), EHomeOwerStatusType.Waiting.ToString()));
            list.Add(new NameValueDto(EHomeOwerStatusType.Done.ToString(), EHomeOwerStatusType.Done.ToString()));
            return list;
        }

        public static EHomeOwerStatusType GetValue(string status)
        {
            if (status == "Initial")
            {
                return EHomeOwerStatusType.Initial;
            }
            else if (status == "Waiting")
            {
                return EHomeOwerStatusType.Waiting;
            }
            else if (status == "Done")
            {
                return EHomeOwerStatusType.Done;
            }
            else
            {
                throw new UserFriendlyException("HomeOwer Status Exception");
            }
        }
    }
}
