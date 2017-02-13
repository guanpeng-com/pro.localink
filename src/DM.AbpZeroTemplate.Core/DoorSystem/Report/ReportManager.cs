using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroDoor.DoorSystem.Enums;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_Reports
    public class ReportManager : DomainService
    {
        public IRepository<Report, long> ReportRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public ReportManager(
         IRepository<Report, long> _ReportRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager
        )
        {

            ReportRepository = _ReportRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Report"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Report entity)
        {
            await ReportRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Report"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Report entity)
        {
            await ReportRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Report"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            var report = await ReportRepository.FirstOrDefaultAsync(id);
            if (report != null && report.Status == EReportStatusType.ReportSend)
            {
                await ReportRepository.DeleteAsync(id);
            }
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Report> FindReportList(string sort)
        {
            var query = from c in ReportRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
