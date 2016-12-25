using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.CMS.Contents.Dto;
using Abp.Contents;
using Abp.AutoMapper;
using Abp.Channels;
using System.Data.Entity;
using Abp.Linq.Extensions;
using Abp.Apps;
using Abp.CMS;
using Abp.Core.Utils;
using Abp.Authorization;
using DM.AbpZeroTemplate.Authorization;
using System.Web;

namespace DM.AbpZeroTemplate.CMS.Contents
{
    public class ContentAppService : AbpZeroTemplateServiceBase, IContentAppService
    {
        private readonly ContentManager _contentManager;
        private readonly ChannelManager _channelManager;
        private readonly AppManager _appManager;

        public ContentAppService(
            ContentManager contentManager,
            ChannelManager channelManager,
            AppManager appManager
            )
        {
            _contentManager = contentManager;
            _channelManager = channelManager;
            _appManager = appManager;
        }

        [AbpAuthorize(AppPermissions.Pages_CMS_Contents_Create)]
        public async Task<ContentDto> CreateContent(CreateContentInput input)
        {
            var content = new Content(input.AppId, input.ChannelId, input.Title, input.ContentText);
            var app = await _appManager.GetByIdAsync(content.AppId);

            content.Author = input.Author;
            content.CheckedLevel = input.CheckedLevel;
            content.Comments = input.Comments;
            content.ContentGroupNameCollection = input.ContentGroupNameCollection;
            content.FileUrl = PageUtils.GetUrlWithoutAppDir(app, input.FileUrl);
            content.Hits = input.Hits;
            content.HitsByDay = input.HitsByDay;
            content.HitsByMonth = input.HitsByMonth;
            content.HitsByWeek = input.HitsByWeek;
            content.ImageUrl = PageUtils.GetUrlWithoutAppDir(app, input.ImageUrl);
            content.IsChecked = input.IsChecked;
            content.IsColor = input.IsColor;
            content.IsHot = input.IsHot;
            content.IsRecommend = input.IsRecommend;
            content.IsTop = input.IsTop;
            content.VideoUrl = PageUtils.GetUrlWithoutAppDir(app, input.VideoUrl);

            await _contentManager.CreateAsync(content);
            //await CurrentUnitOfWork.SaveChangesAsync();
            return content.MapTo<ContentDto>();
        }

        public async Task DeleteContent(IdInput<long> input)
        {
            await _contentManager.DeleteAsync(input.Id);
        }


        public async Task<PagedResultOutput<ContentDto>> GetAllContents(GetContentsInput input)
        {
            return await PrecessGet(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CMS_Contents)]
        public async Task<PagedResultOutput<GetChannelContentDto>> GetContents(GetChannelContentsInput input)
        {
            var channelId = input.Id;
            var query = from con in _contentManager.ContentRepository.GetAll()
                        join ch in _channelManager.ChannelRepository.GetAll() on con.ChannelId equals ch.Id
                        join a in _appManager.AppRepository.GetAll() on ch.AppId equals a.Id
                        where con.ChannelId == input.Id
                        orderby input.Sorting
                        select new { con, ch, a };
            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(c => c.con.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<GetChannelContentDto>(
                totalCount,
                items.Select(
                item =>
                {
                    var dto = item.con.MapTo<GetChannelContentDto>();
                    dto.AppName = item.a.DisplayName;
                    dto.ChannelName = item.ch.DisplayName;
                    return dto;
                }
                ).ToList());
        }

        private async Task<PagedResultOutput<ContentDto>> PrecessGet(GetContentsInput input)
        {
            var query = (IQueryable<Content>)from c in _contentManager.ContentRepository.GetAll()
                                             orderby input.Sorting
                                             select c;

            if (input.ChannelId.HasValue && input.ChannelId.Value > 0)
            {
                query = query.Where(r => r.ChannelId == input.ChannelId.Value);
            }

            if (input.SelectTypes.Count > 0)
                query = query.Where(r => (input.SelectTypes.Contains(0) && r.IsRecommend)
                                                        || (input.SelectTypes.Contains(1) && r.IsTop)
                                                        || (input.SelectTypes.Contains(2) && r.IsColor)
                                                        || (input.SelectTypes.Contains(3) && r.IsHot));

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultOutput<ContentDto>(
                totalCount,
                items.Select(
                        item =>
                        {
                            var dto = item.MapTo<ContentDto>();
                            if (!string.IsNullOrEmpty(dto.ImageUrl))
                                dto.ImageUrl = PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(PathUtils.AddVirtualToPath(PageUtils.GetUrlWithAppDir(item.App, dto.ImageUrl)), HttpContext.Current.Request.Url.Host)).Replace("\\", "/");
                            if (!string.IsNullOrEmpty(dto.FileUrl))
                                dto.FileUrl = PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(PathUtils.AddVirtualToPath(PageUtils.GetUrlWithAppDir(item.App, dto.FileUrl)), HttpContext.Current.Request.Url.Host)).Replace("\\", "/");
                            if (!string.IsNullOrEmpty(dto.VideoUrl))
                                dto.VideoUrl = PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(PathUtils.AddVirtualToPath(PageUtils.GetUrlWithAppDir(item.App, dto.VideoUrl)), HttpContext.Current.Request.Url.Host)).Replace("\\", "/");
                            return dto;
                        }
                    ).ToList()
                );
        }

        [AbpAuthorize(AppPermissions.Pages_CMS_Contents_Move)]
        public async Task<ContentDto> MoveContent(MoveContentInput input)
        {
            if (input.NewChannelId.HasValue)
                await _contentManager.MoveAsync(input.Id, input.NewChannelId.Value);
            var content = await _contentManager.ContentRepository.GetAsync(input.Id);
            return content.MapTo<ContentDto>();
        }


        [AbpAuthorize(AppPermissions.Pages_CMS_Contents_Edit)]
        public async Task<ContentDto> UpdateContent(UpdateContentInput input)
        {
            var content = await _contentManager.ContentRepository.GetAsync(input.Id);
            var app = await _appManager.GetByIdAsync(content.AppId);
            content.Title = input.Title;
            content.ContentText = input.ContentText;
            content.Author = input.Author;
            content.CheckedLevel = input.CheckedLevel;
            content.Comments = input.Comments;
            content.ContentGroupNameCollection = input.ContentGroupNameCollection;
            content.FileUrl = PageUtils.GetUrlWithoutAppDir(app, input.FileUrl);
            content.Hits = input.Hits;
            content.HitsByDay = input.HitsByDay;
            content.HitsByMonth = input.HitsByMonth;
            content.HitsByWeek = input.HitsByWeek;
            content.ImageUrl = PageUtils.GetUrlWithoutAppDir(app, input.ImageUrl);
            content.IsChecked = input.IsChecked;
            content.IsColor = input.IsColor;
            content.IsHot = input.IsHot;
            content.IsRecommend = input.IsRecommend;
            content.IsTop = input.IsTop;
            content.VideoUrl = PageUtils.GetUrlWithoutAppDir(app, input.VideoUrl);

            await _contentManager.UpdateAsync(content);

            await CurrentUnitOfWork.SaveChangesAsync();
            return content.MapTo<ContentDto>();
        }

        /// <summary>
        /// 获取内容信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ContentDto> GetContent(IdInput<long> input)
        {
            var content = await _contentManager.ContentRepository.GetAsync(input.Id);
            var app = await _appManager.GetByIdAsync(content.AppId);
            if (!string.IsNullOrEmpty(content.ImageUrl))
            {
                content.ImageUrl = PageUtils.GetUrlWithAppDir(app, content.ImageUrl);
            }
            if (!string.IsNullOrEmpty(content.VideoUrl))
            {
                content.VideoUrl = PageUtils.GetUrlWithAppDir(app, content.VideoUrl);
            }
            if (!string.IsNullOrEmpty(content.FileUrl))
            {
                content.FileUrl = PageUtils.GetUrlWithAppDir(app, content.FileUrl);
            }
            return content.MapTo<ContentDto>();
        }
    }
}
