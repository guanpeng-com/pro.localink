using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_KeyHoldings
    public class KeyHoldingManager : DomainService
    {
        public IRepository<KeyHolding, long> KeyHoldingRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public KeyHoldingManager(
         IRepository<KeyHolding, long> _KeyHoldingRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager
        )
        {

            KeyHoldingRepository = _KeyHoldingRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="KeyHolding"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(KeyHolding entity)
        {
            await KeyHoldingRepository.InsertAsync(entity);

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="KeyHolding"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(KeyHolding entity)
        {
            await KeyHoldingRepository.UpdateAsync(entity);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="KeyHolding"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await KeyHoldingRepository.DeleteAsync(id);
            var entity = await KeyHoldingRepository.GetAsync(id);

        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<KeyHolding> FindKeyHoldingList(string sort)
        {
            var query = from c in KeyHoldingRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
