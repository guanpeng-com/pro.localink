using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Area
    [AutoMapFrom(typeof(Area))]
    public interface IAreaService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<ListResultOutput<AreaDto>> GetAreas(GetAreasInput input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AreaDto> CreateArea(CreateAreaInput input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AreaDto> UpdateArea(UpdateAreaInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteArea(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AreaDto> GetArea(IdInput<long> input);

        /// <summary>
        /// 获取第一级别的地区
        /// </summary>
        /// <returns></returns>
        Task<List<AreaDto>> GetAreasLevel1();

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<AreaDto>> GetAreasByParentId(IdInput<long?> input);

        /// <summary>
        /// 获取该公司所有的地区
        /// </summary>
        /// <returns></returns>
        Task<List<AreaDto>> GetAllAreas();
    }
}
