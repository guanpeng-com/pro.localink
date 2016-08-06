﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.AutoMapper;
using System.Data.Entity;
using Abp.Linq.Extensions;

using AutoMapper;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Message
    [AutoMapFrom(typeof(Message))]
    public class MessageService : AbpZeroTemplateAppServiceBase, IMessageService
    {
        private readonly MessageManager _manager;
        private readonly HomeOwerManager _homeOwerManager;

        public MessageService(MessageManager manager,
            HomeOwerManager homeOwerManager)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
        }

        public async Task CreateMessage(CreateMessageInput input)
        {
            var entity = new Message(CurrentUnitOfWork.GetTenantId(), input.Title, input.Content, input.CommunityId);
            if (!input.IsPublic)
            {
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);
                entity.HomeOwerId = input.HomeOwerId;
            }
            else
            {
                entity.HomeOwer = null;
                entity.HomeOwerId = null;
            }
            entity.IsRead = input.IsRead;
            entity.IsPublic = input.IsPublic;
            await _manager.CreateAsync(entity);
        }

        public async Task DeleteMessage(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<MessageDto>> GetMessages(GetMessagesInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    var query = _manager.FindMessageList(input.Sorting);

                    if (input.IsPublic.HasValue)
                        query = query.Where(m => m.IsPublic == input.IsPublic.Value);

                    var totalCount = await query.CountAsync();
                    var items = await query.PageBy(input).ToListAsync();
                    return new PagedResultOutput<MessageDto>(
                        totalCount,
                        items.Select(
                                item =>
                                {
                                    var dto = item.MapTo<MessageDto>();
                                    dto.HomeOwerName = item.HomeOwer != null ? item.HomeOwer.Name : "公告";
                                    return dto;
                                }
                            ).ToList()
                        );
                }
            }
        }

        public async Task UpdateMessage(UpdateMessageInput input)
        {
            var entity = await _manager.MessageRepository.GetAsync(input.Id);
            entity.Title = input.Title;
            entity.Content = input.Content;
            entity.HomeOwerId = input.HomeOwerId;
            entity.IsRead = input.IsRead;
            entity.IsPublic = input.IsPublic;
            await _manager.UpdateAsync(entity);
        }

        public async Task<MessageDto> GetMessage(IdInput<long> input)
        {
            var entity = await _manager.MessageRepository.GetAsync(input.Id);
            var dto = Mapper.Map<MessageDto>(entity);
            dto.HomeOwerName = entity.HomeOwer != null ? entity.HomeOwer.Name : "公告";
            return dto;
        }

    }
}
