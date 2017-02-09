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
    //Domain Service Code About Message
    [AutoMapFrom(typeof(Message))]
    public interface IMessageService : IApplicationService
    {
        /// <summary>
        /// 添加信息
        /// ================================
        /// 业主通知：选择具体业主然后添加信息
        /// 1. 记录CommunityId, BuildingId, FlatNoId, HomeOwerId
        /// 2. IsPublic = false
        /// 3. IsRead = false
        /// ================================
        /// 公告：选择单元楼然后添加信息
        /// 1. 记录CommunityId, BuildingId
        /// 2. FlatNoId, HomeOwerId为Null
        /// 3. IsPublic = true
        /// 4. IsRead = null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<MessageDto>> GetMessages(GetMessagesInput input);

        /// <summary>
        /// 获取业主的信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultOutput<MessageDto>> GetAllMessages(GetMessagesInput input);

        /// <summary>
        /// 创建消息通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateMessage(CreateMessageInput input);



        /// <summary>
        /// 修改消息
        /// ================================
        /// 公告
        /// 1. 可以修改的字段有：标题，内容，附件, 状态
        /// ================================
        /// 消息
        /// 2. 可以修改的字段有：标题，内容，附件, 状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateMessage(UpdateMessageInput input);

        /// <summary>
        /// 设置业主消息已读
        /// </summary>
        /// <returns></returns>
        Task<bool> SetIsRead(IdInput<long> input);

        /// <summary>
        /// 删除消息，此接口管只能删除消息，不能删除公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeletePersonalMessage(IdInput<long> input);

        /// <summary>
        /// 删除公告，此接口只对管理端使用，用户端不能删除公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeletePublicMessage(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageDto> GetMessage(IdInput<long> input);

    }
}
