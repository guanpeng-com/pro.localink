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
    /// 业主称谓
    /// </summary>
    public enum EHomeOwerTitleType : byte
    {

        Mr = 1,

        Mrs = 2,

        Miss = 3
    }

    public class EHomeOwerTitleTypeUtils
    {
        public static List<NameValueDto> GetItemCollection()
        {
            List<NameValueDto> list = new List<NameValueDto>();
            list.Add(new NameValueDto(EHomeOwerTitleType.Mr.ToString(), EHomeOwerTitleType.Mr.ToString()));
            list.Add(new NameValueDto(EHomeOwerTitleType.Mrs.ToString(), EHomeOwerTitleType.Mrs.ToString()));
            list.Add(new NameValueDto(EHomeOwerTitleType.Miss.ToString(), EHomeOwerTitleType.Miss.ToString()));
            return list;
        }

        public static EHomeOwerTitleType GetValue(string status)
        {
            if (status == "Mr")
            {
                return EHomeOwerTitleType.Mr;
            }
            else if (status == "Mrs")
            {
                return EHomeOwerTitleType.Mrs;
            }
            else if (status == "Miss")
            {
                return EHomeOwerTitleType.Miss;
            }
            else
            {
                throw new UserFriendlyException("Report Status Exception");
            }
        }
    }
}
