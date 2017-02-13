using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.AutoMapper;
using System.Data.Entity;
using Abp.Linq.Extensions;

using AutoMapper;
using DM.AbpZeroDoor.DoorSystem.Enums;
using DM.AbpZeroTemplate.DoorSystem.Community;
using System;
using DM.AbpZeroTemplate.Core;
using System.Web;
using Abp.Apps;
using Abp.Core.IO;
using Abp.Core.Utils;
using System.IO;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Report
    [AutoMapFrom(typeof(Report))]
    public class ReportService : AbpZeroTemplateAppServiceBase, IReportService
    {
        private readonly ReportManager _manager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly CommunityManager _communityManager;
        private readonly BuildingManager _buildingManager;
        private readonly FlatNumberManager _flatNoManager;
        private readonly AppManager _appManager;
        private readonly IAppFolders _appFolders;

        public ReportService(ReportManager manager,
            HomeOwerManager homeOwerManager,
            CommunityManager communityManager,
            BuildingManager buildingManager,
            FlatNumberManager flatNoManager,
            AppManager appManager,
            IAppFolders appFolders)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
            _communityManager = communityManager;
            _buildingManager = buildingManager;
            _flatNoManager = flatNoManager;
        }

        /// <summary>
        /// 创建保修
        /// ================================
        /// 1. 状态：ReportSend
        /// 2. 没有完成时，完成时间显示 N/A
        /// 3. HandyName暂时手动填写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateReport(CreateReportInput input)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);

            var community = await _communityManager.CommunityRepository.GetAsync(input.CommunityId);
            var building = await _buildingManager.BuildingRepository.GetAsync(input.BuildingId);
            var flatNo = await _flatNoManager.FlatNumberRepository.GetAsync(input.FlatNoId);

            var entity = new Report(CurrentUnitOfWork.GetTenantId(), input.Title, input.Content, input.FileArray, input.CommunityId, input.BuildingId, input.FlatNoId, input.HomeOwerId, community.Name, building.BuildingName, flatNo.FlatNo);
            entity.HomeOwerId = input.HomeOwerId;
            await _manager.CreateAsync(entity);
        }

        /// <summary>
        /// 删除保修单
        /// ================================
        /// 1. 只能删除ReportSend状态的报修记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteReport(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<ReportDto>> GetReports(GetReportsInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    return await ProcessGet(input);
                }
            }
        }

        public async Task<PagedResultOutput<ReportDto>> GetAllReports(GetReportsInput input)
        {
            return await ProcessGet(input);
        }

        private async Task<PagedResultOutput<ReportDto>> ProcessGet(GetReportsInput input)
        {
            //var query = _manager.FindReportList(input.Sorting);
            var query = from r in _manager.ReportRepository.GetAll()
                        orderby input.Sorting
                        select new
                        {
                            r.Id,
                            r.CommunityId,
                            r.BuildingId,
                            r.FlatNoId,
                            r.CommunityName,
                            r.BuildingName,
                            r.FlatNo,
                            r.HandyMan,
                            r.HomeOwer,
                            HomeOwerName = r.HomeOwer.Name,
                            r.HomeOwerId,
                            r.Status,
                            r.Title,
                            r.Content,
                            r.Files,
                            r.CreationTime,
                            CompleteTimeStr = r.CompleteTime.HasValue ? r.CompleteTime.ToString() : "N/A",
                            CreationTimeStr = r.CreationTime.ToString()
                        };

            if (input.HomeOwerId.HasValue)
            {
                //业主ID，用于app端获取数据
                query = query.Where(r => r.HomeOwerId == input.HomeOwerId.Value);
            }

            if (input.CommunityId.HasValue)
            {
                query = query.Where(r => r.CommunityId == input.CommunityId.Value);
            }

            if (!string.IsNullOrEmpty(input.Keywords))
            {
                //单元楼 / 门牌号 / 业主名称
                query = query.Where(m => m.HomeOwer.Forename.Contains(input.Keywords)
                                                            || m.HomeOwer.Surname.Contains(input.Keywords)
                                                            || m.HomeOwer.CommunityName.Contains(input.Keywords)
                                                            || m.BuildingName.Contains(input.Keywords)
                                                            || m.FlatNo.Contains(input.Keywords)
                                                            //|| d.HomeOwer.Buildings.Any(b => b.BuildingName.Contains(input.Keywords))
                                                            );
            }
            if (input.StartDate.HasValue)
            {
                //开始时间
                query = query.Where(m => m.CreationTime >= input.StartDate.Value);
            }
            if (input.EndDate.HasValue)
            {
                input.EndDate = input.EndDate.Value.AddDays(1).AddSeconds(-1);
                //结束时间
                query = query.Where(m => m.CreationTime <= input.EndDate.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<ReportDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            return Mapper.DynamicMap<ReportDto>(item);
                        }
                    ).ToList()
                );
        }

        /// <summary>
        /// 客户端修改保修
        /// ================================
        /// 1. ReportSend状态时，客户端可以维护：标题，内容，图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateReport(UpdateReportInput input)
        {
            var entity = await _manager.ReportRepository.GetAsync(input.Id);
            entity.Title = input.Title;
            entity.Content = input.Content;
            entity.FileArray = input.FileArray;
            await _manager.UpdateAsync(entity);
        }

        /// <summary>
        /// 管理端修改保修
        /// ================================
        /// 1. ReportProcess状态时，管理端维护：状态，处理人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ManageUpdateReport(UpdateReportInput input)
        {
            var entity = await _manager.ReportRepository.GetAsync(input.Id);
            entity.Status = EReportStatusTypeUtils.GetValue(input.Status);
            if (entity.Status == EReportStatusType.ReportProcessing)
            {
                entity.HandyMan = input.HandyMan;
            }
            if (entity.Status == EReportStatusType.ReportFinished)
            {
                //完成时间
                entity.CompleteTime = DateTime.Now;
            }
            await _manager.UpdateAsync(entity);
        }

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ReportDto> GetReport(IdInput<long> input)
        {
            return Mapper.Map<ReportDto>(await _manager.ReportRepository.GetAsync(input.Id));
        }

        /// <summary>
        /// 获取报修状态
        /// </summary>
        /// <returns></returns>
        public List<NameValueDto> GetAllReportStatus()
        {
            return EReportStatusTypeUtils.GetItemCollection();
        }

        /// <summary>
        /// 上传信息附件
        /// </summary>
        /// <returns></returns>
        public async Task<object> UploadFiles(long communityId, [SwaggerFileUpload]string messageFile)
        {

            Community.Community community = null;
            App app = null;

            community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(communityId);

            if (app == null && community != null)
            {
                app = await _appManager.AppRepository.FirstOrDefaultAsync(community.AppId);
            }

            List<string> fileArray = new List<string>();
            var files = HttpContext.Current.Request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var fileName = messageFile;
                if (string.IsNullOrEmpty(fileName))
                    fileName = DateTime.Now.Ticks.ToString();
                fileName = fileName + Path.GetExtension(file.FileName);
                var filePath = PathUtils.Combine(EFileUploadTypeUtils.GetFileUploadPath(EFileUploadType.AppCommon.ToString(), _appFolders, app), "Report", fileName);
                var relateFileUrl = filePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }), string.Empty);
                DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                file.SaveAs(filePath);
                fileArray.Add(relateFileUrl);
            }

            return new { BaseUrl = HttpContext.Current.Request.Url.Host, Files = fileArray };
        }
    }
}
