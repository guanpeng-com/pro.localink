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
    //Domain Service Code About Report
    [AutoMapFrom(typeof(Report))]
    public class ReportService : AbpZeroTemplateAppServiceBase, IReportService
    {
        private readonly ReportManager _manager;
        private readonly HomeOwerManager _homeOwerManager;

        public ReportService(ReportManager manager,
            HomeOwerManager homeOwerManager)
        {
            _manager = manager;
            _homeOwerManager = homeOwerManager;
        }

        public async Task CreateReport(CreateReportInput input)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(input.HomeOwerId);

            var entity = new Report(CurrentUnitOfWork.GetTenantId(), input.Title, input.Content, homeOwer.CommunityId);
            entity.HomeOwerId = input.HomeOwerId;
            entity.FileArray = input.FileArray;
            await _manager.CreateAsync(entity);
        }

        public async Task DeleteReport(IdInput<long> input)
        {
            await _manager.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<ReportDto>> GetReports(GetReportsInput input)
        {
            using (CurrentUnitOfWork.EnableFilter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name))
            {
                using (CurrentUnitOfWork.SetFilterParameter(AbpZeroTemplateConsts.AdminCommunityFilterClass.Name, AbpZeroTemplateConsts.AdminCommunityFilterClass.ParameterName, await GetAdminCommunityIdList()))
                {
                    return await ProcessGet(input);
                }
            }
        }

        public async Task<PagedResultOutput<ReportDto>> GetAllReports(GetReportsInput input)
        {
            return await ProcessGet(input);
        }

        private async Task<PagedResultOutput<ReportDto>> ProcessGet(GetReportsInput input)
        {
            var query = _manager.FindReportList(input.Sorting);

            if (input.HomeOwerId.HasValue)
            {
                query = query.Where(r => r.HomeOwerId == input.HomeOwerId.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<ReportDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            var dto = item.MapTo<ReportDto>();
                            return dto;
                        }
                    ).ToList()
                );
        }

        public async Task UpdateReport(UpdateReportInput input)
        {
            var entity = await _manager.ReportRepository.GetAsync(input.Id);
            entity.Title = input.Title;
            entity.Content = input.Content;
            entity.HomeOwerId = input.HomeOwerId;
            entity.FileArray = input.FileArray;
            entity.Status = EReportStatusTypeUtils.GetValue(input.Status);
            await _manager.UpdateAsync(entity);
        }

        public async Task<ReportDto> GetReport(IdInput<long> input)
        {
            return Mapper.Map<ReportDto>(await _manager.ReportRepository.GetAsync(input.Id));
        }

        public List<NameValueDto> GetAllReportStatus()
        {
            return EReportStatusTypeUtils.GetItemCollection();
        }
    }
}
