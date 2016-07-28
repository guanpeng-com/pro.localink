using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Community
{
    public class CommunityManager : DomainService
    {
        public IRepository<Community, long> CommunityRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public CommunityManager(
            IRepository<Community, long> communityRepository,
            ILogger logger,
            IAbpSession abpSession,
            UserManager userManager
            )
        {
            CommunityRepository = communityRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="community"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Community community)
        {
            community.AuthCommunity();
            await CommunityRepository.InsertAsync(community);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Create Community {1}", currentUser.UserName, community.Name);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="community"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Community community)
        {
            await CommunityRepository.UpdateAsync(community);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Edit Community {1}", currentUser.UserName, community.Name);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="community"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await CommunityRepository.DeleteAsync(id);
            var community = await CommunityRepository.GetAsync(id);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Auth Community {1}", currentUser.UserName, community.Name);
        }

        /// <summary>
        /// 获取小区集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Community> FindCommunityList(string sort)
        {
            var query = from c in CommunityRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }
    }
}
