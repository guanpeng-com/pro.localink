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

    //localink_Buildings
    public class BuildingManager : DomainService
    {
        public IRepository<Building, long> BuildingRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly HomeOwerManager _homeOwerManager;

        public BuildingManager(
         IRepository<Building, long> _BuildingRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager,
         HomeOwerManager homeOwerManager,
         MessageManager messageManager
        )
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
            BuildingRepository = _BuildingRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
            _homeOwerManager = homeOwerManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Building"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Building entity)
        {
            await BuildingRepository.InsertAsync(entity);

            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create Building {1}", currentUser.UserName, entity.BuildingName);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Building"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Building entity)
        {
            await BuildingRepository.UpdateAsync(entity);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create Building {1}", currentUser.UserName, entity.BuildingName);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Building"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await BuildingRepository.DeleteAsync(id);
            var entity = await BuildingRepository.GetAsync(id);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create Building {1}", currentUser.UserName, entity.BuildingName);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Building> FindBuildingList(string sort)
        {
            var query = from c in BuildingRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
