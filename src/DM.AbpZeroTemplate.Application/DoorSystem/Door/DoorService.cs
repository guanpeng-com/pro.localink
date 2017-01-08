using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.AutoMapper;
using System.Data.Entity;
using Abp.Linq.Extensions;
using DM.AbpZeroDoor.DoorSystem.Enums;
using System.Collections;
using DM.AbpZeroTemplate.DoorSystem.Community;
using System;
using AutoMapper;
using Abp.Domain.Repositories;
using DM.AbpZeroTemplate.EntityFramework;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Door
    public class DoorService : AbpZeroTemplateAppServiceBase, IDoorService
    {
        private readonly DoorManager _manager;
        private readonly CommunityManager _communityManager;
        private readonly HomeOwerManager _homeOwerManager;

        public DoorService(DoorManager manager, CommunityManager communityManager, HomeOwerManager homeOwerManager)
        {
            _manager = manager;
            _communityManager = communityManager;
            _homeOwerManager = homeOwerManager;
        }

        public async Task CreateDoor(CreateDoorInput input)
        {
            var community = _communityManager.CommunityRepository.FirstOrDefault(input.CommunityId);
            var entity = new Door(CurrentUnitOfWork.GetTenantId(), community.Id, input.Name, input.PId, input.DepartId);
            entity.DepartId = community.DepartId;
            entity.DoorType = input.DoorType;
            await _manager.CreateAsync(entity);
        }

        public async Task DeleteDoor(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task DeleteHomeOwerDoor(DeleteHomeOwerDoorInput input)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);
            var deletedDoor = new Door();
            foreach (var item in homeOwer.Doors)
            {
                if (item.Id == input.DoorId)
                {
                    deletedDoor = await _manager.DoorRepository.FirstOrDefaultAsync(item.Id);
                    break;
                }
            }
            homeOwer.Doors.Remove(deletedDoor);
            CurrentUnitOfWork.SaveChanges();
        }

        public async Task<PagedResultOutput<DoorDto>> GetDoors(GetDoorsInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    var query = _manager.FindDoorList(input.Sorting);

                    if (input.CommunityId.HasValue)
                    {
                        query = query.Where(d => d.CommunityId == input.CommunityId.Value);
                    }

                    if (input.HomeOwerId.HasValue)
                    {
                        var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId.Value);

                        var ds = homeOwer.Doors.ToList();
                        var dIds = new List<long>();
                        ds.ForEach(d =>
                        {
                            dIds.Add(d.Id);
                        });

                        query = from d in query
                                where dIds.Contains(d.Id)
                                select d;
                    }

                    var totalCount = await query.CountAsync();
                    var items = await query.OrderByDescending(d => d.CreationTime).PageBy(input).ToListAsync();
                    return new PagedResultOutput<DoorDto>(
                        totalCount,
                        items.Select(
                                item =>
                                {
                                    var dto = item.MapTo<DoorDto>();
                                    return dto;
                                }
                            ).ToList()
                        );
                }
            }
        }

        public async Task<List<DoorDto>> GetAllDoors(GetDoorsInput input)
        {
            var query = _manager.FindDoorList(input.Sorting);

            if (input.CommunityId.HasValue)
            {
                query = query.Where(d => d.CommunityId == input.CommunityId.Value);
            }

            if (input.HomeOwerId.HasValue)
            {
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId.Value);

                var ds = homeOwer.Doors.ToList();
                var dIds = new List<long>();
                ds.ForEach(d =>
                {
                    dIds.Add(d.Id);
                });

                query = from d in query
                        where dIds.Contains(d.Id)
                        select d;
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(d => d.CreationTime).ToListAsync();
            List<DoorDto> list = new List<DoorDto>();
            items.ForEach(i =>
            {
                list.Add(Mapper.Map<DoorDto>(i));
            });
            return list;
        }

        public async Task UpdateDoor(UpdateDoorInput input)
        {
            var community = _communityManager.CommunityRepository.FirstOrDefault(input.CommunityId);
            var entity = await _manager.DoorRepository.GetAsync(input.Id);
            entity.Name = input.Name;
            entity.PId = input.PId;
            entity.DepartId = community.DepartId;
            entity.CommunityId = input.CommunityId;
            entity.DoorType = input.DoorType;
            await _manager.UpdateAsync(entity);
        }

        public async Task<DoorDto> GetDoor(IdInput<long> input)
        {
            return Mapper.Map<DoorDto>(await _manager.DoorRepository.GetAsync(input.Id));
        }

        public async Task<DoorDto> AuthDoor(IdInput<long> input)
        {
            var door = await _manager.DoorRepository.GetAsync(input.Id);
            var community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(door.CommunityId);
            if (string.IsNullOrEmpty(door.DepartId))
            {
                door.DepartId = community.DepartId;
            }

            door.AuthDoor();
            await _manager.UpdateAsync(door);
            return Mapper.Map<DoorDto>(door);
        }

        public async Task<ArrayList> GetDoorTypes(IdInput<long> input)
        {
            var community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(input.Id);
            if (community != null)
            {
                var array = community.DoorTypes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return EDoorTypeUtils.GetAllItems(array);
            }
            else
            {
                return EDoorTypeUtils.GetAllItems(new string[0]);
            }
        }

        public async Task<ArrayList> GetCommunityDoorTypes(IdInput<long> input)
        {
            var community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(input.Id);
            ArrayList list = new ArrayList();
            if (community != null)
            {
                var array = community.DoorTypes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string type in array)
                {
                    list.Add(new { key = type, value = L(type) });
                }
            }
            return list;
        }

        public async Task AddHomeOwerDoor(AddHomeOwerDoorInput input)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);
            var door = await _manager.DoorRepository.FirstOrDefaultAsync(input.DoorId);
            homeOwer.Doors.Add(door);
            await _homeOwerManager.UpdateAsync(homeOwer);
        }
    }
}
