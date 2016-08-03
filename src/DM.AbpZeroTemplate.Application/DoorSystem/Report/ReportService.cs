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
    public class ReportService : AbpZeroTemplateServiceBase, IReportService
    {
        private readonly ReportManager _manager;

        public ReportService(ReportManager manager)
        {
            _manager = manager;
        }

        public async Task CreateReport(CreateReportInput input)
        {
            var entity = new Report(CurrentUnitOfWork.GetTenantId(), input.Title, input.Content);
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
            var query = _manager.FindReportList(input.Sorting);

            var totalCount = await query.CountAsync();
            var items = await query.PageBy(input).ToListAsync();
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
