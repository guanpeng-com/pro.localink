using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.Common.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;

namespace DM.AbpZeroTemplate.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultOutput<ComboboxItemDto>> GetEditionsForCombobox();

        Task<PagedResultOutput<NameValueDto>> FindUsers(FindUsersInput input);

        Task<PagedResultOutput<NameValueDto>> FindHomeOwers(FindHomeOwersInput input);

        Task<PagedResultOutput<NameValueDto>> FindDoors(FindDoorsInput input);
    }
}