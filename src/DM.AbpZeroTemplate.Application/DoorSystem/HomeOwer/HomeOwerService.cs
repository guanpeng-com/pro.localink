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
using DM.AbpZeroDoor.DoorSystem.Enums;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About HomeOwer
    public class HomeOwerService : AbpZeroTemplateAppServiceBase, IHomeOwerService
    {
        private readonly HomeOwerManager _manager;
        private readonly DoorManager _doorManager;

        public HomeOwerService(HomeOwerManager manager, DoorManager doorManager)
        {
            _manager = manager;
            _doorManager = doorManager;
        }

        public async Task CreateHomeOwer(CreateHomeOwerInput input)
        {
            var entity = new HomeOwer(CurrentUnitOfWork.GetTenantId(), input.CommunityId, input.Name, input.Phone, input.Email, input.Gender);
            entity.CommunityId = input.CommunityId;

            //录入业主关联的门禁
            entity.Doors = new List<HomeOwerDoor>();
            //小区大门
            var gates = await _doorManager.DoorRepository.GetAllListAsync(d => d.DoorType == EDoorType.Gate.ToString());
            gates.ForEach(door =>
            {
                HomeOwerDoor hd = new HomeOwerDoor(CurrentUnitOfWork.GetTenantId(), entity.Id, door.Id);
                entity.Doors.Add(hd);
            });

            await _manager.CreateAsync(entity);
        }

        public async Task DeleteHomeOwer(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<HomeOwerDto>> GetHomeOwers(GetHomeOwersInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    var query = _manager.FindHomeOwerList(input.Sorting);

                    var totalCount = await query.CountAsync();
                    var items = await query.PageBy(input).ToListAsync();
                    return new PagedResultOutput<HomeOwerDto>(
                        totalCount,
                        items.Select(
                                item =>
                                {
                                    var dto = item.MapTo<HomeOwerDto>();
                                    return dto;
                                }
                            ).ToList()
                        );
                }
            }
        }

        public async Task UpdateHomeOwer(UpdateHomeOwerInput input)
        {
            var entity = await _manager.HomeOwerRepository.GetAsync(input.Id);
            entity.CommunityId = input.CommunityId;
            entity.Name = input.Name;
            entity.Phone = input.Phone;
            entity.Email = input.Email;
            entity.Gender = input.Gender;
            
            await _manager.UpdateAsync(entity);
        }

        public async Task<HomeOwerDto> GetHomeOwer(IdInput<long> input)
        {
            var homeOwer = await _manager.HomeOwerRepository.GetAsync(input.Id);
            var dto = Mapper.Map<HomeOwerDto>(homeOwer);
            return dto;
        }

    }
}
