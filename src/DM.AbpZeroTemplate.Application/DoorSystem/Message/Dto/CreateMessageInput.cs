using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Message
    public class CreateMessageInput : IInputDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 正文
        /// </summary>
        public string Content;

        /// <summary>
        /// 附件集合
        /// </summary>
        public List<string> FileArray;

        /// <summary>
        /// 消息状态：Draft/Sent
        /// </summary>
        public string Status;

        /// <summary>
        /// 单元楼Id集合，公告
        /// </summary>
        public List<long> BuildingIds;

        /// <summary>
        /// 业主Id集合，消息
        /// </summary>
        public List<HomeOwerSelectedInput> HomeOwerIds;

    }
}
