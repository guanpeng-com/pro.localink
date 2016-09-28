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
using System;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About HomeOwer
    public class HomeOwerService : AbpZeroTemplateAppServiceBase, IHomeOwerService
    {
        private readonly HomeOwerManager _manager;
        private readonly HomeOwerUserManager _homeOwerUserManager;
        private readonly DoorManager _doorManager;
        private readonly AccessKeyManager _accessKeyManager;

        public HomeOwerService(HomeOwerManager manager, DoorManager doorManager, AccessKeyManager accessKeyManager, HomeOwerUserManager homeOwerUserManager)
        {
            _manager = manager;
            _doorManager = doorManager;
            _accessKeyManager = accessKeyManager;
            _homeOwerUserManager = homeOwerUserManager;
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

                    if (input.HomeOwerStatus.HasValue)
                    {
                        query = query.Where(h => h.Status == input.HomeOwerStatus.Value);
                    }

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

        public List<NameValueDto> GetAllHomeOwerStatus()
        {
            return EHomeOwerStatusTypeUtils.GetItemCollection();
        }

        /// <summary>
        /// 业主审核，同时发放钥匙
        /// </summary>
        /// <param name="input">业主ID</param>
        /// <returns></returns>
        public async Task AuthHomeOwer(IdInput<long> input)
        {
            var homeOwer = await _manager.HomeOwerRepository.FirstOrDefaultAsync(input.Id);
            var homerOwerUser = await _homeOwerUserManager.HomeOwerUserRepository.FirstOrDefaultAsync(u => u.HomeOwerId == homeOwer.Id);
            var doorIds = from d in homeOwer.Doors
                          select d.DoorId;
            var doors = await _doorManager.DoorRepository.GetAllListAsync(d => doorIds.Contains(d.Id));
            //发放钥匙
            foreach (var door in doors)
            {
                try
                {
                    var key = await _accessKeyManager.AccessKeyRepository.FirstOrDefaultAsync(k => k.HomeOwerId == homeOwer.Id && k.DoorId == door.Id);
                    if (key == null)
                    {
                        key = new AccessKey(CurrentUnitOfWork.GetTenantId(), door.Id, homeOwer.Id, DateTime.Now.AddYears(50), homeOwer.CommunityId);
                        await _accessKeyManager.CreateAsync(key);
                        key.GetKey(door.PId, homeOwer.Phone, key.Validity);
                    }
                    else if (!key.IsAuth)
                    {
                        key.GetKey(door.PId, homeOwer.Phone, key.Validity);
                    }
                }
                catch (Exception)
                {
                    continue;
                }

            }
            //审核业主
            homerOwerUser.IsAuth = true;
            await _homeOwerUserManager.UpdateAsync(homerOwerUser);

            homeOwer.Status = EHomeOwerStatusType.Done;
            homeOwer.AuthTime = DateTime.Now;
            homeOwer.AuthAdmin = base.GetCurrentUser().UserName;
            await _manager.UpdateAsync(homeOwer);
        }
    }
}
