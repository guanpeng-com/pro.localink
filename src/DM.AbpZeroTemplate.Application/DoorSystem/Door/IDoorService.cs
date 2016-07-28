using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DM.AbpZeroTemplate.DoorSystem.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    //Domain Service Code About Door
    [AutoMapFrom(typeof(Door))]
    public interface IDoorService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<DoorDto>> GetDoors(GetDoorsInput input);

        /// <summary>
        /// 获取业主的全部门禁
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DoorDto>> GetAllDoors(GetDoorsInput input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateDoor(CreateDoorInput input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateDoor(UpdateDoorInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteDoor(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DoorDto> GetDoor(IdInput<long> input);

        /// <summary>
        /// 获取门禁类型
        /// </summary>
        /// <returns></returns>
        Task<ArrayList> GetDoorTypes(IdInput<long> input);

        /// <summary>
        /// 获取小区可用的门禁类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ArrayList> GetCommunityDoorTypes(IdInput<long> input);

        /// <summary>
        /// 验证门禁
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DoorDto> AuthDoor(IdInput<long> input);
    }
}
