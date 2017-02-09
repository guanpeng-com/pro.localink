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

    //localink_Deliverys
    public class DeliveryManager : DomainService
    {
        public IRepository<Delivery, long> DeliveryRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly HomeOwerManager _homeOwerManager;
        private readonly MessageManager _messageManager;

        public DeliveryManager(
         IRepository<Delivery, long> _DeliveryRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager,
         HomeOwerManager homeOwerManager,
         MessageManager messageManager
        )
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
            DeliveryRepository = _DeliveryRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
            _homeOwerManager = homeOwerManager;
            _messageManager = messageManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Delivery"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Delivery entity)
        {
            var homeOwer = await _homeOwerManager.HomeOwerRepository.FirstOrDefaultAsync(entity.HomeOwerId);
            if (homeOwer != null)
            {
                //title
                entity.Title = L("DeliveryTitleString", CultureInfo.CurrentUICulture, homeOwer.Name);
                //content
                entity.Content = L("DeliveryContentString", CultureInfo.CurrentUICulture, entity.Token);

                await DeliveryRepository.InsertAsync(entity);

                //添加消息通知
                var message = new Message(CurrentUnitOfWork.GetTenantId(), L("DeliveryTitleMessage", CultureInfo.CurrentUICulture, homeOwer.Name), L("DeliveryContentMessage", CultureInfo.CurrentUICulture, homeOwer.Name), homeOwer.CommunityId, entity.BuildingId, entity.FlatNoId, entity.CommunityName, entity.BuildingName, entity.FlatNo);
                message.HomeOwerId = entity.HomeOwerId;
                await _messageManager.CreateAsync(message);

                var userId = _abpSession.UserId;
                var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
                if (currentUser != null)
                    Logger.InfoFormat("Admin {0} Create Delivery {1}", currentUser.UserName, entity.Title);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Delivery"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Delivery entity)
        {
            await DeliveryRepository.UpdateAsync(entity);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create Delivery {1}", currentUser.UserName, entity.Title);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Delivery"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await DeliveryRepository.DeleteAsync(id);
            var entity = await DeliveryRepository.GetAsync(id);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create Delivery {1}", currentUser.UserName, entity.Title);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Delivery> FindDeliveryList(string sort)
        {
            var query = from c in DeliveryRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

    }
}
