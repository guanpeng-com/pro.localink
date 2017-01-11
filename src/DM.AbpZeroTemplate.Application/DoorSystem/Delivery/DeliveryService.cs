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
using System;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Delivery
    [AutoMapFrom(typeof(Delivery))]
    public class DeliveryService : AbpZeroTemplateAppServiceBase, IDeliveryService
    {
        private readonly DeliveryManager _manager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly BuildingManager _buildingManager;

        public DeliveryService(DeliveryManager manager,
            HomeOwerManager homeOwerManager,
            BuildingManager buildingManager)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
            _buildingManager = buildingManager;
        }

        public async Task CreateDelivery(CreateDeliveryInput input)
        {
            var building = await _buildingManager.BuildingRepository.FirstOrDefaultAsync(input.BuildingId);
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);
            var entity = new Delivery(CurrentUnitOfWork.GetTenantId(), input.HomeOwerId, homeOwer.CommunityId, building.BuildingName);
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
            //var query = _manager.FindDeliveryList(input.Sorting);

            var query = from d in _manager.DeliveryRepository.GetAll()
                        from b in d.HomeOwer.Buildings.DefaultIfEmpty()
                        orderby input.Sorting
                        select new
                        {
                            d.Id,
                            d.CommunityId,
                            CommunityName = b.Community.Name,
                            BuildingId = b.Id,
                            b.BuildingName,
                            d.HomeOwer,
                            d.HomeOwerId,
                            HomeOwerName = d.HomeOwer.Name,
                            d.Title,
                            d.Content,
                            d.IsReplace,
                            d.Token,
                            d.CreationTime,
                            d.IsGather,
                            d.HomeOwer.FlatNo,
                            GatherTime = !d.GatherTime.HasValue ? "N/A" : d.GatherTime.ToString(),
                            ReplaceHomeOwerName = d.ReplaceHomeOwer != null ? d.ReplaceHomeOwer.Name : string.Empty
                        };

            if (input.HomeOwerId.HasValue)
            {
                //业主ID，用于app端获取数据
                query = query.Where(d => d.HomeOwer.Id == input.HomeOwerId.Value);
            }
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                //小区 / 业主名称
                query = query.Where(d => d.HomeOwer.Name.Contains(input.Keywords)
                                                            || d.HomeOwer.CommunityName.Contains(input.Keywords)
                                                            || d.HomeOwer.Buildings.Any(b => b.BuildingName.Contains(input.Keywords))
                                                            );
            }
            if (input.BuildingId.HasValue)
            {
                //单元楼
                query = query.Where(d => d.HomeOwer.Buildings.Any(b => b.Id == input.BuildingId.Value));
            }
            if (input.StartDate.HasValue)
            {
                //开始时间
                query = query.Where(d => d.CreationTime >= input.StartDate.Value);
            }
            if (input.EndDate.HasValue)
            {
                input.EndDate = input.EndDate.Value.AddDays(1).AddSeconds(-1);
                //结束时间
                query = query.Where(d => d.CreationTime <= input.EndDate.Value);
            }
            if (input.IsGather.HasValue)
            {
                //是否收取
                query = query.Where(d => d.IsGather == input.IsGather.Value);
            }
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(d => d.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<DeliveryDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            return Mapper.DynamicMap<DeliveryDto>(item);
                            //var dto = item.MapTo<DeliveryDto>();
                            //return dto;
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
