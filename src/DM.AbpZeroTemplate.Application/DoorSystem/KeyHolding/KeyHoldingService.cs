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

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About KeyHolding
    [AutoMapFrom(typeof(KeyHolding))]
    public class KeyHoldingService : AbpZeroTemplateAppServiceBase, IKeyHoldingService
    {
        private readonly KeyHoldingManager _manager;
        private readonly CommunityManager _communityManager;

        public KeyHoldingService(KeyHoldingManager manager,
            CommunityManager communityManager)
        {
            _manager = manager;
            _communityManager = communityManager;
        }

        public async Task CreateKeyHolding(CreateKeyHoldingInput input)
        {
            var entity = new KeyHolding(CurrentUnitOfWork.GetTenantId(), input.VisitorName, input.VisiteStartTime, input.VisiteEndTime, input.Password, input.KeyType, input.HomeOwerId, input.CommunityId);
            await _manager.CreateAsync(entity);
        }

        public async Task DeleteKeyHolding(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<KeyHoldingDto>> GetKeyHoldings(GetKeyHoldingsInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    return await ProcessGet(input);
                }
            }
        }

        public async Task<PagedResultOutput<KeyHoldingDto>> GetAllKeyHoldings(GetKeyHoldingsInput input)
        {
            return await ProcessGet(input);
        }

        private async Task<PagedResultOutput<KeyHoldingDto>> ProcessGet(GetKeyHoldingsInput input)
        {
            var query = _manager.FindKeyHoldingList(input.Sorting);
            if (input.CommunityId.HasValue)
            {
                query = query.Where(k => k.CommunityId == input.CommunityId.Value);
            }
            if (input.HomeOwerId.HasValue)
            {
                query = query.Where(k => k.HomeOwerId == input.HomeOwerId.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query.PageBy(input).ToListAsync();
            return new PagedResultOutput<KeyHoldingDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            var dto = item.MapTo<KeyHoldingDto>();
                            return dto;
                        }
                    ).ToList()
                );
        }

        public async Task UpdateKeyHolding(UpdateKeyHoldingInput input)
        {
            var entity = await _manager.KeyHoldingRepository.GetAsync(input.Id);
            entity.VisitorName = input.VisitorName;
            entity.VisiteStartTime = input.VisiteStartTime;
            entity.VisiteEndTime = input.VisiteEndTime;
            entity.CollectionTime = input.CollectionTime;
            entity.Password = input.Password;
            entity.IsCollection = input.IsCollection;
            entity.KeyType = input.KeyType;
            entity.HomeOwerId = input.HomeOwerId;
            entity.CommunityId = input.CommunityId;
            await _manager.UpdateAsync(entity);
        }

        public async Task<KeyHoldingDto> GetKeyHolding(IdInput<long> input)
        {

            var entity = await _manager.KeyHoldingRepository.GetAsync(input.Id);
            var dto = Mapper.Map<KeyHoldingDto>(entity);
            dto.HomeOwerName = entity.HomeOwer.Name;
            var community = await _communityManager.CommunityRepository.GetAsync(entity.CommunityId);
            dto.CommunityName = community.Name;
            return dto;
        }

    }
}
