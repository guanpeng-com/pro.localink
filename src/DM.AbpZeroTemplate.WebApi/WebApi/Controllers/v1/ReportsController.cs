using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using DM.AbpZeroTemplate.DoorSystem;
using DM.AbpZeroTemplate.DoorSystem.Community;
using DM.AbpZeroTemplate.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using DM.DoorSystem.Sdk;
using System.Collections;
using Abp.Timing;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.Web.Models;
using Abp.UI;
using DM.AbpZeroTemplate.HttpFormatters;
using DM.AbpZeroDoor.DoorSystem.Enums;
using DM.AbpZeroTemplate.Common.Dto;
using DM.AbpZeroTemplate.Common;
using System.IO;
using Abp.Apps;
using Abp.Core.Utils;
using Abp.Core.IO;
using DM.AbpZeroTemplate.WebApi.Models;
using System.Net;
using System.Net.Http.Formatting;
using DM.AbpZeroTemplate.Core;

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/Reports")]
    [WrapResult]
    public class ReportsController : AbpZeroTemplateApiControllerBase
    {
        private readonly ReportManager _reportManager;
        private readonly CommunityManager _communityManager;
        private readonly AppManager _appManager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly TenantManager _tenantManager;
        private readonly IAppFolders _appFolders;
        private readonly BuildingManager _buildingManager;
        private readonly FlatNumberManager _flatNoManager;

        private readonly ReportService _reportService;

        public ReportsController(
            ReportManager reportManager,
            TenantManager tenantManager,
            HomeOwerUserManager homeOwerUserManager,
            ReportService reportService,
            HomeOwerManager homeOwerManager,
            CommunityManager communityManager,
            BuildingManager buildingManager,
            FlatNumberManager flatNoManager,
            AppManager appManager,
            IAppFolders appFolders)
            : base(homeOwerUserManager)
        {
            _reportManager = reportManager;
            _tenantManager = tenantManager;
            _reportService = reportService;
            _homeOwerManager = homeOwerManager;
            _communityManager = communityManager;
            _buildingManager = buildingManager;
            _flatNoManager = flatNoManager;
            _appManager = appManager;
            _appFolders = appFolders;
        }

        /// <summary>
        /// 获取业主的报修记录集合（分页）
        /// </summary>
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="skipCount">从第 skipCount+1 条开始</param>
        /// <param name="maxResultCount">每页条数</param>
        /// <param name="sorting">排序字段，可传null</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> GetReports(string userName, string token, int skipCount, int maxResultCount, string sorting = null, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);
                if (homeOwerUser.HomeOwerId == 0)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                var input = new GetReportsInput();
                input.HomeOwerId = homeOwerUser.HomeOwerId;
                input.MaxResultCount = maxResultCount;
                input.SkipCount = skipCount;
                if (!string.IsNullOrEmpty(sorting))
                    input.Sorting = sorting;

                return Ok(await _reportService.GetAllReports(input));
            }
        }

        /// <summary>
        /// 业主添加保修单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="createReportModel">post参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> CreateReport(string userName, string token, [FromBody]CreateReportModel createReportModel)
        {
            base.AuthUser();

            var tenantId = createReportModel.TenantId;
            var homeOwerId = createReportModel.HomeOwerId;
            var communityId = createReportModel.CommunityId;
            var title = createReportModel.Title;
            var content = createReportModel.Content;
            var files = createReportModel.Files;

            var community = await _communityManager.CommunityRepository.GetAsync(createReportModel.CommunityId);
            var building = await _buildingManager.BuildingRepository.GetAsync(createReportModel.BuildingId);
            var flatNo = await _flatNoManager.FlatNumberRepository.GetAsync(createReportModel.FlatNoId);

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var report = new Report(tenantId, title, content, files, createReportModel.CommunityId, createReportModel.BuildingId, createReportModel.FlatNoId, createReportModel.HomeOwerId, community.Name, building.BuildingName, flatNo.FlatNo);

                await _reportManager.CreateAsync(report);
                return Ok();
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="reportFile">上传文件名称，可空</param>
        /// <returns>上传文件的路径</returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> UploadFiles(string userName, string token, [SwaggerFileUpload]string reportFile = null)
        {
            base.AuthUser();
            //验证是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }

            var tenantId = base.User.TenantId;
            var homeOwerId = base.User.HomeOwerId;
            var communityId = base.User.CommunityId;

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(homeOwerId.Value);
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                Community community = null;
                App app = null;

                community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(communityId.Value);

                if (app == null && community != null)
                {
                    app = await _appManager.AppRepository.FirstOrDefaultAsync(community.AppId);
                }

                List<string> fileArray = new List<string>();
                var files = HttpContext.Current.Request.Files;


                //保存reportFile, key = reportFile_file
                //var file = files["reportFile_file"];
                //var fileName = reportFile;
                //if (string.IsNullOrEmpty(fileName))
                //    reportFile = DateTime.Now.Ticks.ToString();
                //fileName = fileName + Path.GetExtension(file.FileName);
                //var filePath = PathUtils.Combine(EFileUploadTypeUtils.GetFileUploadPath(EFileUploadType.AppCommon.ToString(), _appFolders, app), fileName);
                //var relateFileUrl = filePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }), string.Empty);
                //DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                //file.SaveAs(filePath);
                //fileArray.Add(relateFileUrl);

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var fileName = reportFile;
                    if (string.IsNullOrEmpty(fileName))
                        fileName = DateTime.Now.Ticks.ToString();
                    fileName = fileName + Path.GetExtension(file.FileName);
                    var filePath = PathUtils.Combine(EFileUploadTypeUtils.GetFileUploadPath(EFileUploadType.AppCommon.ToString(), _appFolders, app), fileName);
                    var relateFileUrl = filePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }), string.Empty);
                    DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                    file.SaveAs(filePath);
                    fileArray.Add(relateFileUrl);
                }

                return Ok(new { BaseUrl = Request.RequestUri.Host, Files = fileArray });
            }
        }

        #region application/json
        ///// <summary>
        ///// 业主添加保修单
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <param name="token">令牌</param>
        ///// <param name="createReportModel">post参数</param>
        ///// <returns></returns>
        //[HttpPost]
        //[UnitOfWork]
        //public async virtual Task<IHttpActionResult> CreateReport(string userName, string token, [FromBody]CreateReportModel createReportModel)
        //{
        //    var tenantId = createReportModel.TenantId;
        //    var homeOwerId = createReportModel.HomeOwerId;
        //    var communityId = createReportModel.CommunityId;
        //    var files = createReportModel.Files;
        //    var title = createReportModel.Title;
        //    var content = createReportModel.Content;
        //    base.AuthUser();
        //    using (CurrentUnitOfWork.SetTenantId(tenantId))
        //    {
        //        var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(homeOwerId);
        //        if (homeOwer == null)
        //        {
        //            throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
        //        }
        //        Community community = null;
        //        App app = null;

        //        community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(communityId);

        //        if (app == null && community != null)
        //        {
        //            app = await _appManager.AppRepository.FirstOrDefaultAsync(community.AppId);
        //        }

        //        List<string> fileArray = new List<string>();
        //        foreach (var file in files)
        //        {
        //            var filePath = EFileUploadTypeUtils.GetFileUploadPath(EFileUploadType.AppCommon.ToString(), _appFolders, app);
        //            var relateFileUrl = PathUtils.Combine(filePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }), string.Empty), file.FileName);
        //            DirectoryUtils.CreateDirectoryIfNotExists(filePath);
        //            file.Save(filePath);
        //            fileArray.Add(relateFileUrl);
        //        }

        //        var report = new Report(tenantId, title, content, communityId);
        //        report.HomeOwerId = homeOwerId;
        //        report.FileArray = fileArray;
        //        await _reportManager.CreateAsync(report);
        //        return Ok();
        //    }
        //} 
        #endregion

    }
}
