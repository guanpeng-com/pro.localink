using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Model Dto Code About Message
    [AutoMapFrom(typeof(Message))]
    public class MessageDto : AuditedEntityDto<long>
    {
        public int TenantId;

        public string Title;

        public string Content;

        public long HomeOwerId;
        public long CommunityId;
        public long BuildingId;
        public long? FlatNoId;

        public string HomeOwerName;
        public string CommunityName;
        public string BuildingName;
        public string FlatNo;

        /// <summary>
        /// 收信方：单元楼/业主
        /// </summary>
        public string To;

        /// <summary>
        /// 是否已读，只针对私信
        /// </summary>
        public bool IsRead;

        /// <summary>
        /// 是否是公告，公告针对Building；私信针对HomeOwer
        /// </summary>
        public bool IsPublic;

        /// <summary>
        /// 附件，多个附件以;隔开
        /// </summary>
        public string Files;

        /// <summary>
        /// 消息状态：草稿/已发送
        /// </summary>
        public string Status;

        /// <summary>
        /// 时间
        /// </summary>
        public string CreationTimeStr;

    }
}
