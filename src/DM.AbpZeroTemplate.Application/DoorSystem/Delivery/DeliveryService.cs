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

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Delivery
    [AutoMapFrom(typeof(Delivery))]
    public class DeliveryService : AbpZeroTemplateAppServiceBase, IDeliveryService
    {
        private readonly DeliveryManager _manager;
        private readonly HomeOwerManager _homeOwerManager;

        public DeliveryService(DeliveryManager manager,
            HomeOwerManager homeOwerManager)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
        }

        public async Task CreateDelivery(CreateDeliveryInput input)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);
            var entity = new Delivery(CurrentUnitOfWork.GetTenantId(), input.HomeOwerId, homeOwer.CommunityId);
            if (!string.IsNullOrEmpty(input.Content))
            {
                entity.Content = input.Content;
            }
            await _manager.CreateAsync(entity);
        }

        public async Task DeleteDelivery(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<DeliveryDto>> GetDeliverys(GetDeliverysInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    return await ProcessGet(input);
                }
            }
        }

        public async Task<PagedResultOutput<DeliveryDto>> GetAllDeliverys(GetDeliverysInput input)
        {
            return await ProcessGet(input);
        }

        private async Task<PagedResultOutput<DeliveryDto>> ProcessGet(GetDeliverysInput input)
        {
            var query = _manager.FindDeliveryList(input.Sorting);
            if (input.HomeOwerId.HasValue)
            {
                query = query.Where(d => d.HomeOwerId == input.HomeOwerId.Value);
            }
            if (!string.IsNullOrEmpty(input.HomeOwerName))
            {
                query = query.Where(d => d.HomeOwer.Name.Contains(input.HomeOwerName));
            }
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(d => d.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<DeliveryDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            var dto = item.MapTo<DeliveryDto>();
                            dto.HomeOwerName = item.HomeOwer.Name;
                            dto.ReplaceHomeOwerName = item.ReplaceHomeOwer != null ? item.ReplaceHomeOwer.Name : string.Empty;
                            return dto;
                        }
                    ).ToList()
                );
        }

        public async Task UpdateDelivery(UpdateDeliveryInput input)
        {
            var entity = await _manager.DeliveryRepository.GetAsync(input.Id);
            entity.HomeOwerId = input.HomeOwerId;
            entity.Content = input.Content;
            await _manager.UpdateAsync(entity);
        }

        public async Task<DeliveryDto> GetDelivery(IdInput<long> input)
        {
            var entity = await _manager.DeliveryRepository.GetAsync(input.Id);
            var dto = Mapper.Map<DeliveryDto>(entity);
            dto.HomeOwerName = entity.HomeOwer.Name;
            dto.ReplaceHomeOwerName = entity.ReplaceHomeOwer != null ? entity.ReplaceHomeOwer.Name : string.Empty;
            return dto;
        }

    }
}
