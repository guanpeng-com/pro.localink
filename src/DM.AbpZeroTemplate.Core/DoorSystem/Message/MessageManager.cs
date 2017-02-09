using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_Messages
    public class MessageManager : DomainService
    {
        public IRepository<Message, long> MessageRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public MessageManager(
         IRepository<Message, long> _MessageRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager
        )
        {

            MessageRepository = _MessageRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Message entity)
        {
            await MessageRepository.InsertAsync(entity);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create Message {1}", currentUser.UserName, entity.Title);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Message entity)
        {
            await MessageRepository.UpdateAsync(entity);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create Message {1}", currentUser.UserName, entity.Title);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public virtual async Task DeletePersonalAsync(long id)
        {
            var entity = await MessageRepository.GetAsync(id);
            if (!entity.IsPublic)
            {
                await MessageRepository.DeleteAsync(id);
                var userId = _abpSession.UserId;
                var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
                if (currentUser != null)
                    Logger.InfoFormat("Admin {0} Create Message {1}", currentUser.UserName, entity.Title);
            }
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public virtual async Task DeletePublicAsync(long id)
        {
            var entity = await MessageRepository.GetAsync(id);
            if (entity.IsPublic)
            {
                await MessageRepository.DeleteAsync(id);
                var userId = _abpSession.UserId;
                var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
                if (currentUser != null)
                    Logger.InfoFormat("Admin {0} Create Message {1}", currentUser.UserName, entity.Title);
            }
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Message> FindMessageList(string sort)
        {
            var query = from c in MessageRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
