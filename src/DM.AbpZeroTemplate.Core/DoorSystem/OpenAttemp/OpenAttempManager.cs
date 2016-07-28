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

        public OpenAttempManager(
         IRepository<OpenAttemp, long> _OpenAttempRepository
        )
        {

            OpenAttempRepository = _OpenAttempRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="OpenAttemp"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(OpenAttemp entity)
        {
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
