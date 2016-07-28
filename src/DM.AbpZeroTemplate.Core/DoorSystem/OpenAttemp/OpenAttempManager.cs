using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_OpenAttemps
    public class OpenAttempManager : DomainService
    {
        public IRepository<OpenAttemp, long> OpenAttempRepository { get; set; }


        private readonly HomeOwerManager _homeOwerManager;
        public IAuditInfoProvider AuditInfoProvider { get; set; }

        public OpenAttempManager(
         IRepository<OpenAttemp, long> _OpenAttempRepository,
            HomeOwerManager homeOwerManager
        )
        {

            OpenAttempRepository = _OpenAttempRepository;
            _homeOwerManager = homeOwerManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="OpenAttemp"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(OpenAttemp entity)
        {
            entity.TenantId = CurrentUnitOfWork.GetTenantId();

            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(entity.HomeOwerId);
            if (homeOwer != null)
            {
                entity.HomeOwerName = homeOwer.Name;
            }

            if (AuditInfoProvider != null)
            {
                var auditInfo = new AuditInfo();
                AuditInfoProvider.Fill(auditInfo);
                entity.BrowserInfo = auditInfo.BrowserInfo;
                entity.ClientIpAddress = auditInfo.ClientIpAddress;
                entity.ClientName = auditInfo.ClientName;
            }

            await OpenAttempRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="OpenAttemp"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(OpenAttemp entity)
        {
            await OpenAttempRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="OpenAttemp"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await OpenAttempRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<OpenAttemp> FindOpenAttempList(string sort)
        {
            var query = from c in OpenAttempRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
