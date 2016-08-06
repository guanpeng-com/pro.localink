using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;
using System;
using Abp.UI;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_HomeOwerUsers
    public class HomeOwerUserManager : DomainService
    {
        public IRepository<HomeOwerUser, long> HomeOwerUserRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public HomeOwerUserManager(
         IRepository<HomeOwerUser, long> _HomeOwerUserRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager
        )
        {

            HomeOwerUserRepository = _HomeOwerUserRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 认证用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool AuthUser(string userName, string token)
        {
            var homeOwerUser = HomeOwerUserRepository.FirstOrDefault(hu => hu.UserName == userName && hu.Token == token);
            return homeOwerUser != null;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="HomeOwerUser"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(HomeOwerUser entity)
        {
            var exists = await HomeOwerUserRepository.FirstOrDefaultAsync(hu => hu.UserName == entity.UserName);
            if (exists != null)
            {
                throw new UserFriendlyException(string.Format("{0} is exists.", entity.UserName));
            }
            await HomeOwerUserRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="HomeOwerUser"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(HomeOwerUser entity)
        {
            await HomeOwerUserRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="HomeOwerUser"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await HomeOwerUserRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<HomeOwerUser> FindHomeOwerUserList(string sort)
        {
            var query = from c in HomeOwerUserRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

        /// <summary>
        /// 根据用户名获取业主用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<HomeOwerUser> GetHomeOwerUserByUserName(string userName)
        {
            var homeOwerUser = await HomeOwerUserRepository.FirstOrDefaultAsync(hu => hu.UserName == userName);
            return homeOwerUser;
        }
    }
}
