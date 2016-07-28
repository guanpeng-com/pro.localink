using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using DM.AbpZeroTemplate.DoorSystem.Community;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_Doors
    public class DoorManager : DomainService
    {
        public IRepository<Door, long> DoorRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly CommunityManager _communityManager;
        private readonly HomeOwerManager _homeOwerManager;

        public DoorManager(
            IRepository<Door, long> _DoorRepository,
            ILogger logger,
            IAbpSession abpSession,
            UserManager userManager,
            CommunityManager communityManager,
            HomeOwerManager homeOwerManager
           )
        {

            DoorRepository = _DoorRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
            _communityManager = communityManager;
            _homeOwerManager = homeOwerManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Door"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Door entity)
        {
            if (string.IsNullOrEmpty(entity.DepartId))
            {
                var community = _communityManager.CommunityRepository.FirstOrDefault(entity.CommunityId);
                entity.DepartId = community.DepartId;
            }
            entity.AuthDoor();
            await DoorRepository.InsertAsync(entity);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Create Door {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Door"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Door entity)
        {
            await DoorRepository.UpdateAsync(entity);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Create Door {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Door"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await DoorRepository.DeleteAsync(id);
            var entity = await DoorRepository.GetAsync(id);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Create Door {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Door> FindDoorList(string sort)
        {
            var query = from c in DoorRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }
    }
}
