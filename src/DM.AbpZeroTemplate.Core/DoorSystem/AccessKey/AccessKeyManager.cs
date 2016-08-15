using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_AccessKeys
    public class AccessKeyManager : DomainService
    {
        public IRepository<AccessKey, long> AccessKeyRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public AccessKeyManager(
         IRepository<AccessKey, long> _AccessKeyRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager
        )
        {
            LocalizationSourceName = DM.AbpZeroTemplate.AbpZeroTemplateConsts.LocalizationSourceName;
            AccessKeyRepository = _AccessKeyRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="AccessKey"></param>
        /// <returns></returns>
        public virtual async Task<AccessKey> CreateAsync(AccessKey entity)
        {
            var isExists = (await AccessKeyRepository.CountAsync(a => a.HomeOwerId == entity.HomeOwerId && a.DoorId == entity.DoorId)) > 0;
            if (!isExists)
            {
                var accessKey = await AccessKeyRepository.InsertAsync(entity);
                var userId = _abpSession.UserId;
                var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
                if (currentUser != null)
                    Logger.InfoFormat("Admin {0} Create AccessKey {1}", currentUser.UserName, entity.Id);
                return accessKey;
            }
            else
            {
                throw new UserFriendlyException("CreateError", L("CreatedAccessKeyIsExists"));
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="AccessKey"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(AccessKey entity)
        {
            var isExists = (await AccessKeyRepository.CountAsync(a => a.Id != entity.Id && a.HomeOwerId == entity.HomeOwerId && a.DoorId == entity.DoorId)) > 0;
            if (!isExists)
            {
                await AccessKeyRepository.UpdateAsync(entity);
                var userId = _abpSession.UserId;
                var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
                if (currentUser != null)
                    Logger.InfoFormat("Admin {0} Create AccessKey {1}", currentUser.UserName, entity.Id);
            }
            else
            {
                throw new UserFriendlyException("CreateError", L("CreatedAccessKeyIsExists"));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="AccessKey"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await AccessKeyRepository.DeleteAsync(id);
            var entity = await AccessKeyRepository.GetAsync(id);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create AccessKey {1}", currentUser.UserName, entity.Id);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<AccessKey> FindAccessKeyList(string sort)
        {
            var query = from c in AccessKeyRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
