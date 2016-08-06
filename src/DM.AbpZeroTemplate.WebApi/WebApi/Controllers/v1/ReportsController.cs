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

        private readonly ReportService _reportService;

        public ReportsController(
            ReportManager reportManager,
            TenantManager tenantManager,
            HomeOwerUserManager homeOwerUserManager,
            ReportService reportService,
            HomeOwerManager homeOwerManager,
            CommunityManager communityManager,
            AppManager appManager,
            IAppFolders appFolders)
            : base(homeOwerUserManager)
        {
            _reportManager = reportManager;
            _tenantManager = tenantManager;
            _reportService = reportService;
            _homeOwerManager = homeOwerManager;
            _communityManager = communityManager;
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
        /// <param name="tenantId">公司Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="token">令牌</param>
        /// <param name="communityId">小区Id</param>
        /// <param name="homeOwerId">业主Id</param>
        /// <param name="title">题目</param>
        /// <param name="content">内容</param>
        /// <param name="files">照片</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> CreateReport(string userName, string token, long communityId, long homeOwerId, string title, string content, List<AppFileUpload> files, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(homeOwerId);
                if (homeOwer == null)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                Community community = null;
                App app = null;

                community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(communityId);

                if (app == null && community != null)
                {
                    app = await _appManager.AppRepository.FirstOrDefaultAsync(community.AppId);
                }

                List<string> fileArray = new List<string>();
                foreach (var file in files)
                {
                    var filePath = EFileUploadTypeUtils.GetFileUploadPath(EFileUploadType.AppCommon.ToString(), _appFolders, app);
                    var relateFileUrl = PathUtils.Combine(filePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }), string.Empty), file.FileName);
                    DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                    file.Save(filePath);
                    fileArray.Add(relateFileUrl);
                }

                var report = new Report(tenantId, title, content, communityId);
                report.HomeOwerId = homeOwerId;
                report.FileArray = fileArray;
                await _reportManager.CreateAsync(report);
                return Ok();
            }
        }

        public class AppFileUpload
        {
            public string FileName { get; set; }
            public byte[] Buffer { get; set; }

            public void Save(string path, string fileName = null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var NewPath = string.Empty;
                if (string.IsNullOrEmpty(fileName))
                    NewPath = Path.Combine(path, FileName);
                else
                    NewPath = Path.Combine(path, fileName);
                if (File.Exists(NewPath))
                {
                    File.Delete(NewPath);
                }

                File.WriteAllBytes(NewPath, Buffer);
            }
        }
    }
}
