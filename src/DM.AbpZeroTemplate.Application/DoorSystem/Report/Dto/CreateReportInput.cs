using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Report
    public class CreateReportInput : IInputDto
    {
        /// <summary>
        /// 保修标题
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
        /// 小区Id
        /// </summary>
        public long CommunityId;

        /// <summary>
        /// 单元Id
        /// </summary>
        public long BuildingId;

        /// <summary>
        /// 门牌号Id
        /// </summary>
        public long FlatNoId;

        /// <summary>
        /// 业主Id
        /// </summary>
        public long HomeOwerId;


    }
}
