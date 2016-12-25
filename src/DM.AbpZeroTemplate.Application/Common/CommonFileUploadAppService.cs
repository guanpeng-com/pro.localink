using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using DM.AbpZeroTemplate.Common.Dto;
using DM.AbpZeroTemplate.Editions;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using AutoMapper;
using DM.AbpZeroTemplate.DoorSystem;
using System;
using System.Collections.Generic;
using System.Web;
using Abp.UI;
using System.IO;
using Abp.Core.Utils;
using Abp.Core.IO;
using DM.AbpZeroDoor.DoorSystem.Enums;
using Abp.Apps;
using Abp.Web.Models;
using DM.AbpZeroTemplate.DoorSystem.Community;
using DM.AbpZeroTemplate.HttpFormatters;

namespace DM.AbpZeroTemplate.Common
{
    [AbpAuthorize]
    public class CommonFileUploadAppService : AbpZeroTemplateAppServiceBase, ICommonFileUploadAppService
    {
        private readonly HomeOwerManager _homeOwerManager;
        private readonly CommunityManager _communityManager;
        private readonly AppManager _appManager;
        private readonly IAppFolders _appFolders;

        public CommonFileUploadAppService(HomeOwerManager homeOwerManager,
            IAppFolders appFolder,
            CommunityManager communityManager,
            AppManager appManager)
        {
            _homeOwerManager = homeOwerManager;
            _appFolders = appFolder;
            _communityManager = communityManager;
            _appManager = appManager;
        }

        public async Task<string> FileUpload(FileUpload<FileUploadInput> input)
        {
            try
            {
                //Check input
                if (HttpContext.Current.Request.Files.Count <= 0 || HttpContext.Current.Request.Files[0] == null)
                {
                    throw new UserFriendlyException(L("FIle_Upload_Error"));
                }

                var file = HttpContext.Current.Request.Files[0];
                var fileInfo = new FileInfo(file.FileName);

                if (input.Value.MaxFileLength != 0 && file.ContentLength > input.Value.MaxFileLength) //  5242880 = 1MB
                {
                    throw new UserFriendlyException(L("FIle_Upload_Warn_SizeLimit"));
                }

                //Check file type
                var fileExtension = fileInfo.Extension;
                if (input.Value.AllowFileExtensionArray.Count > 0 && !input.Value.AllowFileExtensionArray.Contains(fileExtension))
                {
                    throw new ApplicationException(L("FIle_Upload_Warn_ExtensionLimit"));
                }

                Community community = null;
                App app = null;
                if (input.Value.CommunityId.HasValue)
                {
                    community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(input.Value.CommunityId.Value);
                }
                else if (input.Value.HomeOwerId.HasValue)
                {
                    var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.Value.HomeOwerId.Value);
                    if (homeOwer != null)
                        community = homeOwer.Community;
                }
                if (app == null && community != null)
                {
                    app = await _appManager.AppRepository.FirstOrDefaultAsync(community.AppId);
                }

                //Save new picture
                //string fileName = string.Format("{0}{1}", Guid.NewGuid(), fileInfo.Extension);
                var filePath = EFileUploadTypeUtils.GetFileUploadPath(input.Value.FileUploadType, _appFolders, app);
                var relateFileUrl = PathUtils.Combine(filePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }), string.Empty), input.FileName);
                DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                input.Save(filePath);
                return relateFileUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
