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
using DM.AbpZeroTemplate.DoorSystem.Community;
using System;
using DM.AbpZeroTemplate.Core;
using System.Web;
using Abp.Core.Utils;
using System.IO;
using DM.AbpZeroDoor.DoorSystem.Enums;
using Abp.Apps;
using Abp.Core.IO;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Message
    [AutoMapFrom(typeof(Message))]
    public class MessageService : AbpZeroTemplateAppServiceBase, IMessageService
    {
        private readonly MessageManager _manager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly CommunityManager _communityManager;
        private readonly BuildingManager _buildingManager;
        private readonly FlatNumberManager _flatNoManager;
        private readonly AppManager _appManager;
        private readonly IAppFolders _appFolders;

        public MessageService(MessageManager manager,
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
            _appManager = appManager;
            _appFolders = appFolders;
        }

        /// <summary>
        /// 添加信息
        /// ================================
        /// 业主通知：选择具体业主然后添加信息
        /// 1. 记录CommunityId, BuildingId, FlatNoId, HomeOwerId
        /// 2. IsPublic = false
        /// 3. IsRead = false
        /// ================================
        /// 公告：选择单元楼然后添加信息
        /// 1. 记录CommunityId, BuildingId
        /// 2. FlatNoId, HomeOwerId为Null
        /// 3. IsPublic = true
        /// 4. IsRead = null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateMessage(CreateMessageInput input)
        {
            #region 公告
            if (input.BuildingIds != null)
            {
                foreach (long buildingId in input.BuildingIds)
                {
                    var building = await _buildingManager.BuildingRepository.GetAsync(buildingId);
                    if (building != null)
                    {
                        var entity = new Message(CurrentUnitOfWork.GetTenantId(), input.Title, input.Content, input.FileArray, input.Status, building.CommunityId, buildingId);
                        await _manager.CreateAsync(entity);
                    }
                }
            }
            #endregion

            #region 消息
            if (input.BuildingIds != null)
            {
                foreach (var homeOwerDto in input.HomeOwerIds)
                {
                    var community = await _communityManager.CommunityRepository.GetAsync(homeOwerDto.CommunityId);
                    var building = await _buildingManager.BuildingRepository.GetAsync(homeOwerDto.BuildingId);
                    var flatNo = await _flatNoManager.FlatNumberRepository.GetAsync(homeOwerDto.FlatNoId);
                    if (community != null && building != null && flatNo != null)
                    {
                        var entity = new Message(CurrentUnitOfWork.GetTenantId(), input.Title, input.Content, input.FileArray, input.Status, homeOwerDto.CommunityId, homeOwerDto.BuildingId, homeOwerDto.FlatNoId, homeOwerDto.HomeOwerId, community.Name, building.BuildingName, flatNo.FlatNo);
                        await _manager.CreateAsync(entity);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 删除消息，此接口管只能删除消息，不能删除公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeletePersonalMessage(IdInput<long> input)
        {
            await _manager.DeletePersonalAsync(input.Id);
        }

        /// <summary>
        /// 删除公告，此接口只对管理端使用，用户端不能删除公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeletePublicMessage(IdInput<long> input)
        {
            await _manager.DeletePublicAsync(input.Id);
        }

        public async Task<PagedResultOutput<MessageDto>> GetMessages(GetMessagesInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    return await ProcessGet(input);
                }
            }
        }

        public async Task<PagedResultOutput<MessageDto>> GetAllMessages(GetMessagesInput input)
        {
            return await ProcessGet(input);
        }

        private async Task<PagedResultOutput<MessageDto>> ProcessGet(GetMessagesInput input)
        {
            //var query = _manager.FindMessageList(input.Sorting);

            var query = from m in _manager.MessageRepository.GetAll()
                        orderby input.Sorting
                        select new
                        {
                            To = m.IsPublic ? m.BuildingName : m.HomeOwer.Name,
                            m.Id,
                            m.Title,
                            m.Content,
                            m.IsPublic,
                            m.IsRead,
                            m.CreationTime,
                            m.HomeOwer,
                            m.Files,
                            HomeOwerName = m.HomeOwer.Name,
                            m.CommunityName,
                            m.BuildingName,
                            m.FlatNo,
                            m.HomeOwerId,
                            m.Status,
                            m.CommunityId,
                            m.BuildingId,
                            m.FlatNoId,
                            CreationTimeStr = m.CreationTime.ToString()
                        };

            if (input.HomeOwerId.HasValue)
            {
                //业主ID，用于app端获取数据
                query = query.Where(r => r.HomeOwerId == input.HomeOwerId.Value);
            }

            if (input.CommunityId.HasValue)
            {
                query = query.Where(m => m.CommunityId == input.CommunityId.Value);
            }

            if (!string.IsNullOrEmpty(input.Keywords))
            {
                //单元楼 / 门牌号 / 业主名称
                query = query.Where(m => m.HomeOwer.Name.Contains(input.Keywords)
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
            if (!string.IsNullOrEmpty(input.Status))
            {
                //状态
                query = query.Where(m => m.Status == input.Status);
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(m => m.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<MessageDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            return Mapper.DynamicMap<MessageDto>(item);
                        }
                    ).ToList()
                );
        }

        /// <summary>
        /// 修改消息
        /// ================================
        /// 公告
        /// 1. 可以修改的字段有：标题，内容，附件, 状态
        /// ================================
        /// 消息
        /// 2. 可以修改的字段有：标题，内容，附件, 状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateMessage(UpdateMessageInput input)
        {
            var entity = await _manager.MessageRepository.GetAsync(input.Id);
            entity.Title = input.Title;
            entity.Content = input.Content;
            entity.FileArray = input.Files;
            entity.Status = input.Status;
            await _manager.UpdateAsync(entity);
        }

        public async Task<MessageDto> GetMessage(IdInput<long> input)
        {
            var entity = await _manager.MessageRepository.GetAsync(input.Id);
            var dto = Mapper.Map<MessageDto>(entity);
            return dto;
        }

        /// <summary>
        /// 设置业主消息已读
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SetIsRead(IdInput<long> input)
        {
            var entity = await _manager.MessageRepository.GetAsync(input.Id);
            if (!entity.IsPublic)
            {
                entity.IsRead = true;
                await _manager.UpdateAsync(entity);
            }
            return true;
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
                var filePath = PathUtils.Combine(EFileUploadTypeUtils.GetFileUploadPath(EFileUploadType.AppCommon.ToString(), _appFolders, app), fileName);
                var relateFileUrl = filePath.Replace(System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }), string.Empty);
                DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                file.SaveAs(filePath);
                fileArray.Add(relateFileUrl);
            }

            return new { BaseUrl = HttpContext.Current.Request.Url.Host, Files = fileArray };
        }
    }
}
