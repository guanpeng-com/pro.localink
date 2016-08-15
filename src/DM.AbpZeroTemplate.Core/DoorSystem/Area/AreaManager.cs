using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using DM.AbpZeroTemplate.Authorization.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using DM.AbpZeroTemplate.Configuration;
using Abp.Linq.Extensions;
using System.Data.Entity;

namespace DM.AbpZeroTemplate.DoorSystem
{

    //localink_Areas
    public class AreaManager : DomainService
    {
        public IRepository<Area, long> AreaRepository { get; set; }
        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;

        public AreaManager(
         IRepository<Area, long> _AreaRepository,
         ILogger logger,
         IAbpSession abpSession,
         UserManager userManager
        )
        {

            AreaRepository = _AreaRepository;
            Logger = logger;
            _abpSession = abpSession;
            _userManager = userManager;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Area"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(Area entity)
        {
            //设置父级路径
            await ProcessParentPath(entity);

            await AreaRepository.InsertAsync(entity);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Create Area {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Area"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Area entity)
        {
            //设置父级路径
            await ProcessParentPath(entity);

            await AreaRepository.UpdateAsync(entity);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Create Area {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Area"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long id)
        {
            //var area = await AreaRepository.FirstOrDefaultAsync(id);
            //string parentPath = string.Join(Area.ParentPathSplitString, area.ParentPath, area.Id.ToString()).Trim(new char[] { ',' });

            //await AreaRepository.DeleteAsync(a => a.ParentPath == parentPath);
            //await AreaRepository.DeleteAsync(a => a.ParentPath.Contains(parentPath + Area.ParentPathSplitString));


            List<long> deletedIds = await GetChildIdByParentId(id);
            AreaRepository.Delete(a => deletedIds.Contains(a.Id));
            await AreaRepository.DeleteAsync(id);
            var entity = await AreaRepository.GetAsync(id);
            var userId = _abpSession.GetUserId();
            var currentUser = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            Logger.InfoFormat("Admin {0} Create Area {1}", currentUser.UserName, entity.Name);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Area> FindAreaList(string sort)
        {
            var query = from c in AreaRepository.GetAll()
                        orderby sort
                        select c;
            return query;
        }

        public async virtual Task<List<long>> GetChildIdByParentId(long parentId)
        {
            List<long> cidList = new List<long>();
            Area area = await AreaRepository.FirstOrDefaultAsync(parentId);
            foreach (var item in area.Children)
            {
                cidList.Add(item.Id);
            }

            foreach (var item in area.Children)
            {
                cidList.AddRange(await GetChildIdByParentId(item.Id));
            }
            return cidList;
        }

        public virtual async Task ProcessParentPath(Area entity)
        {
            if (entity.ParentId.HasValue)
            {
                var parent = await AreaRepository.FirstOrDefaultAsync(entity.ParentId.Value);
                if (parent != null)
                {
                    entity.ParentPath = string.Join(Area.ParentPathSplitString, parent.ParentPath, parent.Id.ToString()).Trim(Area.ParentPathSplitString.ToCharArray());
                }
            }
            else
            {
                entity.ParentPath = null;
            }
        }

        public virtual async Task<List<Area>> GetAllAreas()
        {
            var query = AreaRepository.GetAll();
            //需要判断该租户设置的国家是哪个
            var rootAreaIdStr = await SettingManager.GetSettingValueAsync(AppSettings.UserManagement.RootAreaId);
            long rootAreaId = 0;
            if (long.TryParse(rootAreaIdStr, out rootAreaId))
            {
                query = query.Where(a => a.ParentId == rootAreaId);
            }
            query.Include("Children");
            var list = await query.ToListAsync();
            return list;
        }
    }
}
