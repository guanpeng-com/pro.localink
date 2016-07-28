using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.AutoMapper;
using System.Data.Entity;
using Abp.Linq.Extensions;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About OpenAttemp
    [AutoMapFrom(typeof(OpenAttemp))]
    public class OpenAttempService : AbpZeroTemplateServiceBase, IOpenAttempService
    {
        private readonly OpenAttempManager _manager;

        public OpenAttempService(OpenAttempManager manager)
        {
            _manager = manager;
        }

        public async Task CreateOpenAttemp(CreateOpenAttempInput input)
        {
            var entity = new OpenAttemp();
            entity.HomeOwerId = input.HomeOwerId;
            entity.UserName = input.UserName;
            await _manager.CreateAsync(entity);
        }

        public async Task DeleteOpenAttemp(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<OpenAttempDto>> GetOpenAttemps(GetOpenAttempsInput input)
        {
            var query = _manager.FindOpenAttempList(input.Sorting);

            var totalCount = await query.CountAsync();
            var items = await query.PageBy(input).ToListAsync();
            return new PagedResultOutput<OpenAttempDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            var dto = item.MapTo<OpenAttempDto>();
                            return dto;
                        }
                    ).ToList()
                );
        }

        public async Task<OpenAttemp> GetOpenAttemp(IdInput<long> input)
        {
            return await _manager.OpenAttempRepository.GetAsync(input.Id);
        }

    }
}
