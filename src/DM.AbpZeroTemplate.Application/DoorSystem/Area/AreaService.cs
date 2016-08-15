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
using System.Collections;
using DM.AbpZeroTemplate.Authorization;
using Abp.Authorization;
using DM.AbpZeroTemplate.Configuration;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Area
    [AutoMapFrom(typeof(Area))]
    public class AreaService : AbpZeroTemplateServiceBase, IAreaService
    {
        private readonly AreaManager _manager;

        public AreaService(AreaManager manager)
        {
            _manager = manager;
        }

        public async Task<AreaDto> CreateArea(CreateAreaInput input)
        {
            var entity = new Area(input.ParentId, input.Name);
            await _manager.CreateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return Mapper.Map<AreaDto>(entity);
        }

        public async Task DeleteArea(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<ListResultOutput<AreaDto>> GetAreas(GetAreasInput input)
        {
            var query = _manager.FindAreaList(input.Sorting);

            var totalCount = await query.CountAsync();
            var items = await query.PageBy(input).ToListAsync();
            return new ListResultOutput<AreaDto>(
                items.Select(
                        item =>
                        {
                            var dto = item.MapTo<AreaDto>();
                            return dto;
                        }
                    ).ToList()
                );
        }

        public async Task<AreaDto> UpdateArea(UpdateAreaInput input)
        {
            var entity = await _manager.AreaRepository.GetAsync(input.Id);
            entity.Name = input.Name;
            entity.ParentId = input.ParentId;
            await _manager.UpdateAsync(entity);

            return Mapper.Map<AreaDto>(entity);
        }

        public async Task<AreaDto> GetArea(IdInput<long> input)
        {
            return Mapper.Map<AreaDto>(await _manager.AreaRepository.GetAsync(input.Id));
        }

        public async Task<List<AreaDto>> GetAreasLevel1()
        {
            return await GetAreasByParentId(new IdInput<long?> { Id = null });
        }

        public async Task<List<AreaDto>> GetAreasByParentId(IdInput<long?> input)
        {
            List<Area> list = new List<Area>();
            list = await _manager.AreaRepository.GetAllListAsync(a => a.ParentId == input.Id);
            return Mapper.Map<List<AreaDto>>(list);
        }

        public async Task<List<AreaDto>> GetAllAreas()
        {
            var list = await _manager.GetAllAreas();
            return Mapper.Map<List<AreaDto>>(list);
        }
    }
}
