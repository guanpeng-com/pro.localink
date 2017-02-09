using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DM.AbpZeroDoor.DoorSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    /// <summary>
    /// 小区业主
    /// </summary>
    [Table("localink_HomeOwers")]
    public class HomeOwer : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {
        #region 构造函数
        public HomeOwer()
        {
        }

        public HomeOwer(int? tenantId, long communityId, string name, string phone, string email, string gender, string communityName)
        {
            TenantId = tenantId;
            CommunityId = communityId;
            Name = name;
            Phone = phone;
            Email = email;
            Gender = gender;
            CommunityName = communityName;
            Status = EHomeOwerStatusType.Initial;
            Deliverys = new List<Delivery>();
            Messages = new List<Message>();
            Reports = new List<Report>();
        }
        #endregion

        #region 字段属性
        public const int MaxDefaultStringLength = 50;
        #endregion

        #region 外键
        /// <summary>
        /// 门禁集合
        /// </summary>
        public virtual ICollection<Door> Doors { get; set; }

        /// <summary>
        /// 钥匙
        /// </summary>
        public virtual ICollection<AccessKey> AccessKeys { get; set; }
        /// <summary>
        /// 快递集合
        /// </summary>
        public virtual ICollection<Delivery> Deliverys { get; set; }
        /// <summary>
        /// 信息集合
        /// </summary>
        public virtual ICollection<Message> Messages { get; set; }
        /// <summary>
        /// 保修集合
        /// </summary>
        public virtual ICollection<Report> Reports { get; set; }
        /// <summary>
        /// 门牌号集合
        /// </summary>
        public virtual ICollection<FlatNumber> FlatNumbers { get; set; }
        #endregion

        #region 基本属性
        /// <summary>
        /// 小区Id，冗余字段，localink_Building.CommunityId
        /// </summary>
        public virtual long CommunityId { get; set; }

        /// <summary>
        /// 小区名称，冗余字段
        /// </summary>
        public virtual string CommunityName { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

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

        /// <summary>
        /// 业主状态
        /// </summary>
        [Required]
        public virtual EHomeOwerStatusType Status { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        public virtual DateTime? AuthTime { get; set; }

        /// <summary>
        /// 审核管理员
        /// </summary>
        public virtual string AuthAdmin { get; set; }
        
        #endregion
    }
}
