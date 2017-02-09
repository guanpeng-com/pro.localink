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
    //Domain Service Code About Building
    [AutoMapFrom(typeof(Building))]
    public class BuildingService : AbpZeroTemplateAppServiceBase, IBuildingService
    {
        private readonly BuildingManager _manager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly BuildingManager _buildingManager;
        private readonly FlatNumberManager _flatNoManager;

        public BuildingService(BuildingManager manager,
            HomeOwerManager homeOwerManager,
            BuildingManager buildingManager,
            FlatNumberManager flatNumberManager)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
            _buildingManager = buildingManager;
            _flatNoManager = flatNumberManager;
        }

        /// <summary>
        /// 添加快递信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BuildingDto> CreateBuilding(CreateBuildingInput input)
        {
            var entity = new Building(CurrentUnitOfWork.GetTenantId(), input.CommunityId, input.BuildingName);

            await _manager.CreateAsync(entity);
            return Mapper.Map<BuildingDto>(entity);
        }

        public async Task DeleteBuilding(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<BuildingDto>> GetBuildings(GetBuildingsInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    return await ProcessGet(input);
                }
            }
        }

        public async Task<PagedResultOutput<BuildingDto>> GetAllBuildings(GetBuildingsInput input)
        {
            return await ProcessGet(input);
        }

        private async Task<PagedResultOutput<BuildingDto>> ProcessGet(GetBuildingsInput input)
        {
            //var query = _manager.FindBuildingList(input.Sorting);

            var query = from d in _manager.BuildingRepository.GetAll()
                        orderby input.Sorting
                        select new
                        {
                            d.Id,
                            d.CommunityId,
                            CommunityName = d.Community.Name,
                            d.BuildingName,
                            d.CreationTime,
                            CreationTimeStr = d.CreationTime.ToString()
                        };

            if (input.CommunityId.HasValue)
            {
                //业主ID，用于app端获取数据
                query = query.Where(d => d.CommunityId == input.CommunityId.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(d => d.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<BuildingDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            return Mapper.DynamicMap<BuildingDto>(item);
                            //var dto = item.MapTo<BuildingDto>();
                            //return dto;
                        }
                    ).ToList()
                );
        }

        public async Task UpdateBuilding(UpdateBuildingInput input)
        {
            var entity = await _manager.BuildingRepository.GetAsync(input.Id);
            entity.CommunityId = input.CommunityId;
            entity.BuildingName = input.BuildingName;
            await _manager.UpdateAsync(entity);
        }

        public async Task<BuildingDto> GetBuilding(IdInput<long> input)
        {
            var entity = await _manager.BuildingRepository.GetAsync(input.Id);
            var dto = Mapper.Map<BuildingDto>(entity);
            return dto;
        }

    }
}
