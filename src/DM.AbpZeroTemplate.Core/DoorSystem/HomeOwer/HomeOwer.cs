using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    [Table("localink_HomeOwers")]
    public class HomeOwer : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {
        public HomeOwer()
        {
        }

        public HomeOwer(int? tenantId, long communityId, string name, string phone, string email, string gender)
        {
            TenantId = TenantId;
            CommunityId = communityId;
            Name = name;
            Phone = phone;
            Email = email;
            Gender = gender;
        }

        public const int MaxDefaultStringLength = 50;


        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 小区Id
        /// </summary>
        public virtual long CommunityId { get; set; }

        /// <summary>
        /// 小区
        /// </summary>
        [ForeignKey("CommunityId")]
        public virtual Community.Community Community { get; set; }

        /// <summary>
        /// 门禁集合
        /// </summary>
        public virtual ICollection<HomeOwerDoor> Doors { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(MaxDefaultStringLength)]
        public virtual string Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(MaxDefaultStringLength)]
        public virtual string Gender { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [StringLength(MaxDefaultStringLength)]
        public virtual string ValidateCode { get; set; }
    }
}
