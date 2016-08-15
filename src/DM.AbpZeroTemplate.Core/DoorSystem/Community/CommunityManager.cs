using Abp.Apps;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Community
{
    public class CommunityManager : DomainService
    {
        public IRepository<Community, long> CommunityRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly AppManager _appManager;

        public CommunityManager(
            IRepository<Community, long> communityRepository,
            ILogger logger,
            IAbpSession abpSession,
            UserManager userManager,
            AppManager appManager
            )
        {
            CommunityRepository = communityRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
            _appManager = appManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="community"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Community community)
        {
            //创建小区之前，需要创建小区的cms
            App app = new App(community.TenantId, community.Name, community.Name, "APP_" + Guid.NewGuid().ToString());
            await _appManager.CreateAsync(app);
            CurrentUnitOfWork.SaveChanges();
            community.AppId = app.Id;

            community.AuthCommunity();
            await CommunityRepository.InsertAsync(community);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
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
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
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
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
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

        /// <summary>
        /// 根据经纬度，获取小区集合
        /// </summary>
        /// <param name="lat">经度</param>
        /// <param name="lng">纬度</param>
        /// <param name="raidus">半径</param>
        /// <param name="name">小区名称(模糊查询参数)</param>
        /// <returns></returns>
        public virtual IQueryable<Community> FindCommunityListByLatLng(double lat, double lng, int raidus, string name = null)
        {
            var latLngArr = LatLngUtils.getAround(lat, lng, raidus);
            var minLat = latLngArr[0];
            var maxLat = latLngArr[2];
            var minLng = latLngArr[1];
            var maxLng = latLngArr[3];
            var query = FindCommunityList(string.Empty);
            query = query.Where(
                c =>
                c.IsAuth &&
                c.Lat >= minLat && c.Lat <= maxLat && c.Lng >= minLng && c.Lng <= maxLng);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }
            return query;
        }
    }
}
