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
        /// 获取
        /// </summary>
        /// <returns></returns>
        Task<PagedResultOutput<MessageDto>> GetMessages(GetMessagesInput input);

        /// <summary>
        /// 创建消息通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateMessage(CreateMessageInput input);



        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateMessage(UpdateMessageInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteMessage(IdInput<long> input);

        /// <summary>
        /// 根据Id获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageDto> GetMessage(IdInput<long> input);

    }
}
