using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using DM.AbpZeroTemplate.Common.Dto;
using DM.AbpZeroTemplate.Editions;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using AutoMapper;
using DM.AbpZeroTemplate.DoorSystem;

namespace DM.AbpZeroTemplate.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : AbpZeroTemplateAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        private readonly HomeOwerManager _homeOwerManager;

        public CommonLookupAppService(EditionManager editionManager,
            HomeOwerManager homeOwerManager)
        {
            _editionManager = editionManager;
            _homeOwerManager = homeOwerManager;
        }

        public async Task<ListResultOutput<ComboboxItemDto>> GetEditionsForCombobox()
        {
            var editions = await _editionManager.Editions.ToListAsync();
            return new ListResultOutput<ComboboxItemDto>(
                editions.Select(e => new ComboboxItemDto(e.Id.ToString(), e.DisplayName)).ToList()
                );
        }

        public async Task<PagedResultOutput<NameValueDto>> FindUsers(FindUsersInput input)
        {
            using (UnitOfWorkManager.Current.SetTenantId(input.TenantId))
            {
                if (AbpSession.MultiTenancySide == MultiTenancySides.Host && input.TenantId.HasValue)
                {
                    CurrentUnitOfWork.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, input.TenantId.Value);
                }

                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    );

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultOutput<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.Name + " " + u.Surname + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );

            }
        }

        public async Task<PagedResultOutput<NameValueDto>> FindHomeOwers(FindHomeOwersInput input)
        {

            var query = _homeOwerManager.HomeOwerRepository.GetAll()
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Phone.Contains(input.Filter) ||
                        u.Email.Contains(input.Filter)
                );

            var homeOwerCount = await query.CountAsync();
            var homeOwers = await query
                .OrderBy(u => u.Name)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultOutput<NameValueDto>(
                homeOwerCount,
                homeOwers.Select(u =>
                    new NameValueDto(
                        u.Name + " (" + u.Phone + " | " + u.Email + ")",
                        u.Id.ToString()
                        )
                    ).ToList()
                );

        }
    }
}
