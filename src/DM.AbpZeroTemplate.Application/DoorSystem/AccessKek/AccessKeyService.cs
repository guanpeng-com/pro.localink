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
using Abp.UI;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About AccessKey
    public class AccessKeyService : AbpZeroTemplateAppServiceBase, IAccessKeyService
    {
        private readonly AccessKeyManager _manager;
        private readonly CommunityManager _communityManager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly DoorManager _doorManager;

        public AccessKeyService(
            AccessKeyManager manager,
            CommunityManager communityManager,
            HomeOwerManager homeOwerManager,
            DoorManager doorManager)
        {
            _manager = manager;
            _communityManager = communityManager;
            _homeOwerManager = homeOwerManager;
            _doorManager = doorManager;
        }

        public async Task CreateAccessKey(CreateAccessKeyInput input)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);
            var door = await _doorManager.DoorRepository.FirstOrDefaultAsync(input.DoorId);
            var entity = new AccessKey(CurrentUnitOfWork.GetTenantId(), door, homeOwer, input.Validity, homeOwer.CommunityId);
            var accessKey = await _manager.CreateAsync(entity);


            accessKey.GetKey(door.PId, homeOwer.Phone, accessKey.Validity);
        }

        public async Task DeleteAccessKey(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<AccessKeyDto>> GetAccessKeys(GetAccessKeysInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    var query = _manager.FindAccessKeyList(input.Sorting);

                    var homeOwerDic = new Dictionary<long, string>();
                    var doorDic = new Dictionary<long, string>();

                    var totalCount = await query.CountAsync();
                    var items = await query.PageBy(input).ToListAsync();

                    items.ForEach(i =>
                    {
                        if (!homeOwerDic.Keys.Contains(i.HomeOwer.Id))
                            homeOwerDic.Add(i.HomeOwer.Id, string.Empty);
                        if (!doorDic.Keys.Contains(i.Door.Id))
                            doorDic.Add(i.Door.Id, string.Empty);
                    });

                    var homeOwerNames = from h in _homeOwerManager.HomeOwerRepository.GetAll()
                                        where homeOwerDic.Keys.Contains(h.Id)
                                        select new { h.Id, h.Name };
                    await homeOwerNames.ForEachAsync(h =>
                                         {
                                             homeOwerDic[h.Id] = h.Name;
                                         });

                    var doorNames = from d in _doorManager.DoorRepository.GetAll()
                                    where doorDic.Keys.Contains(d.Id)
                                    select new { d.Id, d.Name };
                    await doorNames.ForEachAsync(d =>
                    {
                        doorDic[d.Id] = d.Name;
                    });


                    return new PagedResultOutput<AccessKeyDto>(
                        totalCount,
                        items.Select(
                                item =>
                                {
                                    var dto = item.MapTo<AccessKeyDto>();
                                    dto.HomeOwerName = homeOwerDic[item.HomeOwer.Id];
                                    dto.DoorName = doorDic[item.Door.Id];
                                    return dto;
                                }
                            ).ToList()
                        );
                }
            }
        }

        public async Task UpdateAccessKey(UpdateAccessKeyInput input)
        {
            var entity = await _manager.AccessKeyRepository.GetAsync(input.Id);
            var door = await _doorManager.DoorRepository.FirstOrDefaultAsync(input.DoorId);
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);
            if (!entity.IsAuth)
            {
                entity.Door = door;
                entity.HomeOwer = homeOwer;
            }
            entity.Validity = input.Validity;
            await _manager.UpdateAsync(entity);
        }

        public async Task<AccessKeyDto> GetAccessKey(IdInput<long> input)
        {
            return Mapper.Map<AccessKeyDto>(await _manager.AccessKeyRepository.GetAsync(input.Id));
        }

        public async Task<AccessKeyDto> AuthAccessKey(IdInput<long> input)
        {
            var accessKey = await _manager.AccessKeyRepository.FirstOrDefaultAsync(input.Id);
            var door = await _doorManager.DoorRepository.FirstOrDefaultAsync(accessKey.Door.Id);
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(accessKey.HomeOwer.Id);

            accessKey.GetKey(door.PId, homeOwer.Phone, accessKey.Validity);
            await _manager.UpdateAsync(accessKey);
            return Mapper.Map<AccessKeyDto>(accessKey);
        }

    }
}
