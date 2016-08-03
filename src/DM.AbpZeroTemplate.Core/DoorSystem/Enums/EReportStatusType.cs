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
    /// 保修状态枚举
    /// </summary>
    public enum EReportStatusType : byte
    {
        /// <summary>
        /// 提交
        /// </summary>
        ReportSend = 1,
        /// <summary>
        /// 处理中
        /// </summary>
        ReportProcessing = 2,
        /// <summary>
        /// 已完成
        /// </summary>
        ReportFinished = 3
    }

    public class EReportStatusTypeUtils
    {
        public static List<NameValueDto> GetItemCollection()
        {
            List<NameValueDto> list = new List<NameValueDto>();
            list.Add(new NameValueDto(EReportStatusType.ReportSend.ToString(), EReportStatusType.ReportSend.ToString()));
            list.Add(new NameValueDto(EReportStatusType.ReportProcessing.ToString(), EReportStatusType.ReportProcessing.ToString()));
            list.Add(new NameValueDto(EReportStatusType.ReportFinished.ToString(), EReportStatusType.ReportFinished.ToString()));
            return list;
        }

        public static EReportStatusType GetValue(string status)
        {
            if (status == "ReportSend")
            {
                return EReportStatusType.ReportSend;
            }
            else if (status == "ReportProcessing")
            {
                return EReportStatusType.ReportProcessing;
            }
            else if (status == "ReportFinished")
            {
                return EReportStatusType.ReportFinished;
            }
            else
            {
                throw new UserFriendlyException("Report Status Exception");
            }
        }
    }
}
