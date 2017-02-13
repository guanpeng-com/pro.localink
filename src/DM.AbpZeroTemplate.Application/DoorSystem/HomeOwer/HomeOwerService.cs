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
using DM.AbpZeroTemplate.DoorSystem.Community;
using Abp.UI;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About HomeOwer
    public class HomeOwerService : AbpZeroTemplateAppServiceBase, IHomeOwerService
    {
        private readonly CommunityManager _communityManager;
        private readonly BuildingManager _buildingManager;
        private readonly HomeOwerManager _manager;
        private readonly DoorManager _doorManager;
        private readonly AccessKeyManager _accessKeyManager;

        public HomeOwerService(HomeOwerManager manager, DoorManager doorManager, AccessKeyManager accessKeyManager, CommunityManager communityManager, BuildingManager buildingManager)
        {
            _manager = manager;
            _doorManager = doorManager;
            _accessKeyManager = accessKeyManager;
            _communityManager
 = communityManager;
            _buildingManager = buildingManager;
        }

        /// <summary>
        /// 添加业主信息
        /// ================================
        /// 1. 业主录入初始状态：Initial
        /// 2. 业主录入，默认自带小区大门的门禁
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateHomeOwer(CreateHomeOwerInput input)
        {
            var community = await _communityManager.CommunityRepository.FirstOrDefaultAsync(input.CommunityId);

            var entity = new HomeOwer(CurrentUnitOfWork.GetTenantId(), input.CommunityId, input.Forename, input.Surname, input.Phone, input.Email, EHomeOwerTitleTypeUtils.GetValue(input.Title), EHomeOwerGroupTypeUtils.GetValue(input.UserGroup), community.Name, input.AltContace, input.AltMobile);

            //录入业主关联的门禁
            entity.Doors = new List<Door>();
            //小区大门
            var gates = await _doorManager.DoorRepository.GetAllListAsync(d => d.DoorType == EDoorType.Gate.ToString());

            gates.ForEach(door =>
            {
                entity.Doors.Add(door);
            });

            await _manager.CreateAsync(entity);
        }

        /// <summary>
        /// 删除业主
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteHomeOwer(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 查询业主
        /// ===============================
        /// 1. 查询条件：CommunityId, CommunityName, BuildingId, BuildingName, HomeOwerName, FlatNo, UserGroup, Status
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultOutput<HomeOwerDto>> GetHomeOwers(GetHomeOwersInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    // var query = _manager.FindHomeOwerList(input.Sorting);

                    var query = from h in _manager.HomeOwerRepository.GetAll()
                                from f in h.FlatNumbers.DefaultIfEmpty()
                                where h.IsLock == false && h.CommunityId == input.CommunityId
                                select new
                                {
                                    h.Id,
                                    h.CommunityName,
                                    h.CommunityId,
                                    f.Building.BuildingName,
                                    BuildingId = f.BuildingId,
                                    FlatNoId = f.Id,
                                    f.FlatNo,
                                    HomeOwerName = h.Name,
                                    h.Forename,
                                    h.Surname,
                                    h.Status,
                                    h.Phone,
                                    h.Email,
                                    h.Title,
                                    h.CreationTime,
                                    h.UserGroup,
                                    AccessStatus = h.Status == EHomeOwerStatusType.Done ? "Active" : "N/A",
                                    CreationTimeStr = h.CreationTime.ToString()
                                };


                    //业主状态
                    if (input.Status.HasValue)
                    {
                        query = query.Where(h => h.Status == input.Status.Value);
                    }
                    //单元楼
                    if (input.BuildingId.HasValue)
                    {
                        query = query.Where(h => h.BuildingId == input.BuildingId.Value);
                    }
                    //小区，单元楼，门牌号，业主名称
                    if (!string.IsNullOrEmpty(input.Keywords))
                    {
                        query = query.Where(h =>
                        h.CommunityName.Contains(input.Keywords)
                        || h.Forename.Contains(input.Keywords)
                        || h.Surname.Contains(input.Keywords)
                        || h.BuildingName.Contains(input.Keywords)
                        || h.FlatNo.Contains(input.Keywords));
                    }
                    //业主类型
                    if (!string.IsNullOrEmpty(input.UserGroup))
                    {
                        query = query.Where(h => h.UserGroup.ToString() == input.UserGroup);
                    }

                    var totalCount = await query.CountAsync();
                    var items = await query.OrderByDescending(h => h.CreationTime).PageBy(input).ToListAsync();
                    return new PagedResultOutput<HomeOwerDto>(
                        totalCount,
                        items.Select(
                                item =>
                                {
                                    return Mapper.DynamicMap<HomeOwerDto>(item);
                                    //var dto = item.MapTo<HomeOwerDto>();
                                    //return dto;
                                }
                            ).ToList()
                        );
                }
            }
        }

        /// <summary>
        /// 修改业主消息
        /// ================================
        /// 1. 可以维护的字段：Forename, Surname, Phone, Email, Title, AltContact, AltMobile
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateHomeOwer(UpdateHomeOwerInput input)
        {
            var entity = await _manager.HomeOwerRepository.GetAsync(input.Id);
            //entity.CommunityId = input.CommunityId;
            entity.Forename = input.Forename;
            entity.Surname = input.Surname;
            entity.Phone = input.Phone;
            entity.Email = input.Email;
            entity.Title = EHomeOwerTitleTypeUtils.GetValue(input.Title);
            entity.AltContact = input.AltContact;
            entity.AltMobile = input.AltMobile;

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

            if (homeOwer.IsLock)
            {
                throw new UserFriendlyException(L("HomeOwerIsLocked"));
            }

            var doorIds = from d in homeOwer.Doors
                          select d.Id;
            var doors = await _doorManager.DoorRepository.GetAllListAsync(d => doorIds.Contains(d.Id));
            //发放钥匙
            foreach (var door in doors)
            {
                try
                {
                    var key = await _accessKeyManager.AccessKeyRepository.FirstOrDefaultAsync(k => k.HomeOwer.Id == homeOwer.Id && k.Door == door);
                    if (key == null)
                    {
                        key = new AccessKey(CurrentUnitOfWork.GetTenantId(), door, homeOwer, DateTime.Now.AddYears(50), homeOwer.CommunityId);
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

            homeOwer.Status = EHomeOwerStatusType.Done;
            homeOwer.AuthTime = DateTime.Now;
            homeOwer.AuthAdmin = base.GetCurrentUser().UserName;
            await _manager.UpdateAsync(homeOwer);
        }

        /// <summary>
        /// 锁定业主
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> LockHomeOwer(IdInput<long> input)
        {
            var homeOwer = await _manager.HomeOwerRepository.FirstOrDefaultAsync(input.Id);
            if (homeOwer != null && !homeOwer.IsLock)
            {
                homeOwer.IsLock = true;
                await _manager.UpdateAsync(homeOwer);
            }
            return true;
        }

        /// <summary>
        /// 解锁业主
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UnLockHomeOwer(IdInput<long> input)
        {
            var homeOwer = await _manager.HomeOwerRepository.FirstOrDefaultAsync(input.Id);
            if (homeOwer != null && homeOwer.IsLock)
            {
                homeOwer.IsLock = false;
                await _manager.UpdateAsync(homeOwer);
            }
            return true;
        }
    }
}
