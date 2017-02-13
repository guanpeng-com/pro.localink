using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem.Dto
{
    //Create Model Dto Code About HomeOwer
    public class CreateHomeOwerInput : IInputDto
    {
        /// <summary>
        /// 小区Id
        /// </summary>
        public long CommunityId;

        /// <summary>
        /// 名
        /// </summary>
        public string Forename;

        /// <summary>
        /// 姓
        /// </summary>
        public string Surname;

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email;

        /// <summary>
        /// 称谓
        /// </summary>
        public string Title;

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string AltContace;

        /// <summary>
        /// 紧急联系电话
        /// </summary>
        public string AltMobile;

        /// <summary>
        /// 业主类型：ManagingAgent/Owner/OwnerOccupier/Tenant
        /// </summary>
        public string UserGroup;


        //public long GarageId;

        //public long MailboxId;

        //public long MaindoorId;

    }
}
