using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About Delivery
    public class CreateDeliveryInput : IInputDto
    {
        /// <summary>
        /// 条形码
        /// </summary>
        public string Barcode;

        /// <summary>
        /// 小区Id
        /// </summary>
        public long CommunityId;

        /// <summary>
        /// 单元Id
        /// </summary>
        public long BuildingId;

        /// <summary>
        /// 门牌号
        /// </summary>
        public long FlatNoId;

        /// <summary>
        /// 业主Id
        /// </summary>
        public long HomeOwerId;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content;
    }
}
