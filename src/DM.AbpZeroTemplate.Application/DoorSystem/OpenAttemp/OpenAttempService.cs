﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using Abp.AutoMapper;
using System.Data.Entity;
using Abp.Linq.Extensions;
using Abp.Auditing;
using AutoMapper;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About OpenAttemp
    [AutoMapFrom(typeof(OpenAttemp))]
    public class OpenAttempService : AbpZeroTemplateAppServiceBase, IOpenAttempService
    {

        public IAuditInfoProvider AuditInfoProvider { get; set; }

        private readonly OpenAttempManager _manager;
        private readonly HomeOwerManager _homeOwerManager;

        public OpenAttempService(OpenAttempManager manager,
            HomeOwerManager homeOwerManager)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
        }

        public async Task CreateOpenAttemp(CreateOpenAttempInput input)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);

            var entity = new OpenAttemp(CurrentUnitOfWork.GetTenantId(), homeOwer.CommunityId);
            entity.HomeOwerId = input.HomeOwerId;
            entity.UserName = input.UserName;
            entity.IsSuccess = input.IsSuccess;

            await _manager.CreateAsync(entity);
        }

        public async Task DeleteOpenAttemp(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<OpenAttempDto>> GetOpenAttemps(GetOpenAttempsInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    var query = _manager.FindOpenAttempList(input.Sorting);

                    var totalCount = await query.CountAsync();
                    var items = await query.OrderByDescending(o => o.CreationTime).PageBy(input).ToListAsync();
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
            }
        }

        public async Task<OpenAttempDto> GetOpenAttemp(IdInput<long> input)
        {
            return Mapper.Map<OpenAttempDto>(await _manager.OpenAttempRepository.GetAsync(input.Id));
        }

    }
}
