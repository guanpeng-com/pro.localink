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
    public class AccessKeyService : AbpZeroTemplateServiceBase, IAccessKeyService
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
            var isExists = (await _manager.AccessKeyRepository.CountAsync(a => a.HomeOwerId == input.HomeOwerId && a.DoorId == input.DoorId)) > 0;
            if (!isExists)
            {
                var entity = new AccessKey(CurrentUnitOfWork.GetTenantId(), input.DoorId, input.HomeOwerId, input.Validity);

                var door = await _doorManager.DoorRepository.FirstOrDefaultAsync(entity.DoorId);
                var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(entity.HomeOwerId);

                entity.GetKey(door.PId, homeOwer.Phone, entity.Validity);

                await _manager.CreateAsync(entity);
            }
            else
            {
                throw new UserFriendlyException("CreateError", L("CreatedAccessKeyIsExists"));
            }
        }

        public async Task DeleteAccessKey(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<AccessKeyDto>> GetAccessKeys(GetAccessKeysInput input)
        {
            var query = _manager.FindAccessKeyList(input.Sorting);

            var homeOwerDic = new Dictionary<long, string>();
            var doorDic = new Dictionary<long, string>();

            var totalCount = await query.CountAsync();
            var items = await query.PageBy(input).ToListAsync();

            items.ForEach(i =>
            {
                if (!homeOwerDic.Keys.Contains(i.HomeOwerId))
                    homeOwerDic.Add(i.HomeOwerId, string.Empty);
                if (!doorDic.Keys.Contains(i.DoorId))
                    doorDic.Add(i.DoorId, string.Empty);
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
                            dto.HomeOwerName = homeOwerDic[item.HomeOwerId];
                            dto.DoorName = doorDic[item.DoorId];
                            return dto;
                        }
                    ).ToList()
                );
        }

        public async Task UpdateAccessKey(UpdateAccessKeyInput input)
        {
            var entity = await _manager.AccessKeyRepository.GetAsync(input.Id);

            if (!entity.IsAuth)
            {
                entity.DoorId = input.DoorId;
                entity.HomeOwerId = input.HomeOwerId;
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
            var door = await _doorManager.DoorRepository.FirstOrDefaultAsync(accessKey.DoorId);
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(accessKey.HomeOwerId);

            accessKey.GetKey(door.PId, homeOwer.Phone, accessKey.Validity);
            await _manager.UpdateAsync(accessKey);
            return Mapper.Map<AccessKeyDto>(accessKey);
        }

    }
}
