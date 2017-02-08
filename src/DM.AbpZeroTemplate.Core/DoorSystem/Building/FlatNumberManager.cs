using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_FlatNumbers
    public class FlatNumberManager : DomainService
    {
        public IRepository<FlatNumber, long> FlatNumberRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly HomeOwerManager _homeOwerManager;

        public FlatNumberManager(
         IRepository<FlatNumber, long> _FlatNumberRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager,
         HomeOwerManager homeOwerManager,
         MessageManager messageManager
        )
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
            FlatNumberRepository = _FlatNumberRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
            _homeOwerManager = homeOwerManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FlatNumber"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(FlatNumber entity)
        {
            await FlatNumberRepository.InsertAsync(entity);

            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create FlatNumber {1}", currentUser.UserName, entity.FlatNo);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="FlatNumber"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(FlatNumber entity)
        {
            await FlatNumberRepository.UpdateAsync(entity);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create FlatNumber {1}", currentUser.UserName, entity.FlatNo);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="FlatNumber"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await FlatNumberRepository.DeleteAsync(id);
            var entity = await FlatNumberRepository.GetAsync(id);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create FlatNumber {1}", currentUser.UserName, entity.FlatNo);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<FlatNumber> FindFlatNumberList(string sort)
        {
            var query = from c in FlatNumberRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
