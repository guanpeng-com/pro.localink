﻿using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_HomeOwers
    public class HomeOwerManager : DomainService
    {
        public IRepository<HomeOwer, long> HomeOwerRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public HomeOwerManager(
         IRepository<HomeOwer, long> _HomeOwerRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager
        )
        {

            HomeOwerRepository = _HomeOwerRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="HomeOwer"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(HomeOwer entity)
        {
            await HomeOwerRepository.InsertAsync(entity);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create HomeOwer {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="HomeOwer"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(HomeOwer entity)
        {
            await HomeOwerRepository.UpdateAsync(entity);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create HomeOwer {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="HomeOwer"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            await HomeOwerRepository.DeleteAsync(id);
            var entity = await HomeOwerRepository.GetAsync(id);
            var userId = _abpSession.UserId;
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (currentUser != null)
                Logger.InfoFormat("Admin {0} Create HomeOwer {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<HomeOwer> FindHomeOwerList(string sort)
        {
            var query = from c in HomeOwerRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

        /// <summary>
        /// 根据名称和手机号获取业主
        /// </summary>
        /// <param name="homeOwerName"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public virtual async Task<HomeOwer> GetHomeOwerByNameAndPhone(string homeOwerName, string phone)
        {
            var homeOwer = await HomeOwerRepository.FirstOrDefaultAsync(h => h.Name == homeOwerName && h.Phone == phone);
            return homeOwer;
        }

        /// <summary>
        /// 根据名称和手机号获取业主
        /// </summary>
        /// <param name="communityId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public virtual async Task<HomeOwer> GetHomeOwerByNameAndPhoneAndCommunityId(long communityId, string phone)
        {
            var homeOwer = await HomeOwerRepository.FirstOrDefaultAsync(h => h.Phone == phone && h.CommunityId == communityId);
            return homeOwer;
        }
    }
}
