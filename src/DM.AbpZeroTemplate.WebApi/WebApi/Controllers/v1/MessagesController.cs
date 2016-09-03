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

namespace DM.AbpZeroTemplate.WebApi.Controllers.v1
{
    [VersionedRoute("api/version", 1)]
    [RoutePrefix("api/v1/Messages")]
    [WrapResult]
    public class MessagesController : AbpZeroTemplateApiControllerBase
    {
        private readonly MessageManager _messageManager;
        private readonly CommunityManager _communityManager;
        private readonly AppManager _appManager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly TenantManager _tenantManager;
        private readonly IAppFolders _appFolders;

        private readonly MessageService _messageService;

        public MessagesController(
            MessageManager messageManager,
            TenantManager tenantManager,
            HomeOwerUserManager homeOwerUserManager,
            MessageService messageService,
            HomeOwerManager homeOwerManager,
            CommunityManager communityManager,
            AppManager appManager,
            IAppFolders appFolders)
            : base(homeOwerUserManager)
        {
            _messageManager = messageManager;
            _tenantManager = tenantManager;
            _messageService = messageService;
            _homeOwerManager = homeOwerManager;
            _communityManager = communityManager;
            _appManager = appManager;
            _appFolders = appFolders;
        }

        /// <summary>
        /// 获取业主的消息记录集合（分页）
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
        public async virtual Task<IHttpActionResult> GetMessages(string userName, string token, int skipCount, int maxResultCount, string sorting = null, int? tenantId = null)
        {
            base.AuthUser();
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var homeOwerUser = await _homeOwerUserManager.GetHomeOwerUserByUserName(userName);
                if (homeOwerUser.HomeOwerId == 0)
                {
                    throw ErrorCodeTypeUtils.ThrowError(ErrorCodeType.HomeOwerNotExists);
                }
                var input = new GetMessagesInput();
                input.HomeOwerId = homeOwerUser.HomeOwerId;
                input.MaxResultCount = maxResultCount;
                input.SkipCount = skipCount;
                if (!string.IsNullOrEmpty(sorting))
                    input.Sorting = sorting;

                return Ok(await _messageService.GetAllMessages(input));
            }
        }
    }
}
