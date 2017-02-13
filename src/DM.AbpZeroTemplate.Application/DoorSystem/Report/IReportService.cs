using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using DM.AbpZeroTemplate.Core;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Report
    [AutoMapFrom(typeof(Report))]
    public interface IReportService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<ReportDto>> GetReports(GetReportsInput input);

        /// <summary>
        /// 获取业主的快递
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<ReportDto>> GetAllReports(GetReportsInput input);

        /// <summary>
        /// 创建保修
        /// ================================
        /// 1. 状态：ReportSend
        /// 2. 没有完成时，完成时间显示 N/A
        /// 3. HandyName暂时手动填写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateReport(CreateReportInput input);

        /// <summary>
        /// 客户端修改保修
        /// ================================
        /// 1. ReportSend状态时，客户端可以维护：标题，内容，图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateReport(UpdateReportInput input);

        /// <summary>
        /// 管理端修改保修
        /// ================================
        /// 1. ReportProcess状态时，管理端维护：状态，处理人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ManageUpdateReport(UpdateReportInput input);

        /// <summary>
        /// 删除保修单
        /// ================================
        /// 1. 只能删除ReportSend状态的报修记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteReport(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ReportDto> GetReport(IdInput<long> input);

        /// <summary>
        /// 获取报修状态
        /// </summary>
        /// <returns></returns>
        List<NameValueDto> GetAllReportStatus();

        /// <summary>
        /// 上传保修附件
        /// </summary>
        /// <param name="communityId"></param>
        /// <param name="messageFile"></param>
        /// <returns></returns>
        Task<object> UploadFiles(long communityId, [SwaggerFileUpload]string messageFile);
    }
}
