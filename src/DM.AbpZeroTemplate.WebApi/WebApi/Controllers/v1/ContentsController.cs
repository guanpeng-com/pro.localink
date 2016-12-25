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
using Abp.Contents;
using DM.AbpZeroTemplate.CMS.Contents.Dto;
using DM.AbpZeroTemplate.CMS.Contents;

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/Contents")]
    [WrapResult]
    public class ContentsController : AbpZeroTemplateApiControllerBase
    {

        private readonly CommunityManager _communityManager;
        private readonly AppManager _appManager;
        private readonly HomeOwerUserManager _homeOwerUserManager;
        private readonly TenantManager _tenantManager;
        private readonly ContentManager _contentManager;
        private readonly ContentAppService _contentServices;


        private readonly ReportService _reportService;

        public ContentsController(
            HomeOwerUserManager homeOwerUserManager,
            TenantManager tenantManager,
            CommunityManager communityManager,
            AppManager appManager,
            ContentManager contentManager,
            ContentAppService contentAppService)
            : base(homeOwerUserManager)
        {
            _homeOwerUserManager = homeOwerUserManager;
            _tenantManager = tenantManager;
            _communityManager = communityManager;
            _appManager = appManager;
            _contentManager = contentManager;
            _contentServices = contentAppService;
        }

        /// <summary>
        /// 获取首页轮播内容集合（分页）
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="userName">用户名</param>
        /// <param name="inputDto">用户名</param>    
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async virtual Task<IHttpActionResult> GetLunBoContents(string userName, string token, GetLunBoContentsInputDto inputDto)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(inputDto.TenantId))
            {
                var community = _communityManager.CommunityRepository.Get(inputDto.CommunityId);

                GetContentsInput input = new GetContentsInput();
                input.ChannelId = inputDto.ChannelId;
                input.MaxResultCount = inputDto.MaxResultCount;
                input.Sorting = inputDto.Sorting;
                input.SkipCount = inputDto.SkipCount;
                input.SelectTypes = inputDto.SelectTypes;

                return Ok(await _contentServices.GetAllContents(input));
            }
        }
    }
}
