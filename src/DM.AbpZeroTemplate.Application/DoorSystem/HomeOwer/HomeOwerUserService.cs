using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.AutoMapper;
using System.Data.Entity;
using Abp.Linq.Extensions;
using Abp.EntityFramework.Extensions;
using AutoMapper;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About HomeOwerUser
    [AutoMapFrom(typeof(HomeOwerUser))]
    public class HomeOwerUserService : AbpZeroTemplateAppServiceBase, IHomeOwerUserService
    {
        private readonly HomeOwerUserManager _manager;
        private readonly HomeOwerManager _homeOwerManager;

        public HomeOwerUserService(HomeOwerUserManager manager,
            HomeOwerManager homeOwerManager)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
        }

        public async Task CreateHomeOwerUser(CreateHomeOwerUserInput input)
        {
            var entity = new HomeOwerUser(input.UserName, input.Token);
            await _manager.CreateAsync(entity);
        }

        public async Task DeleteHomeOwerUser(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<HomeOwerUserDto>> GetHomeOwerUsers(GetHomeOwerUsersInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    var query = _manager.FindHomeOwerUserList(input.Sorting);
                    query.Include("HomeOwer");
                    var totalCount = await query.CountAsync();
                    var items = await query.OrderByDescending(h => h.CreationTime).PageBy(input).ToListAsync();
                    return new PagedResultOutput<HomeOwerUserDto>(
                        totalCount,
                        items.Select(
                                item =>
                                {
                                    var dto = item.MapTo<HomeOwerUserDto>();
                                    return dto;
                                }
                            ).ToList()
                        );
                }
            }
        }

        public async Task UpdateHomeOwerUser(UpdateHomeOwerUserInput input)
        {
            var entity = await _manager.HomeOwerUserRepository.GetAsync(input.Id);
            entity.HomeOwerId = input.HomeOwerId;
            entity.UserName = input.UserName;
            await _manager.UpdateAsync(entity);
        }

        public async Task<HomeOwerUserDto> GetHomeOwerUser(IdInput<long> input)
        {
            return Mapper.Map<HomeOwerUserDto>(await _manager.HomeOwerUserRepository.GetAsync(input.Id));
        }

    }
}
