using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    /// <summary>
    /// 小区单元楼
    /// </summary>
    [Table("localink_Buildings")]
    public class Building : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {
        #region 构造函数
        public Building() { }
        #endregion

        #region 外键
        /// <summary>
        /// 小区，M to 1
        /// </summary>
        public long CommunityId { get; set; }
        [ForeignKey("CommunityId")]
        public virtual Community.Community Community { get; set; }

        /// <summary>
        /// 业主集合, M to M
        /// </summary>
        public virtual ICollection<HomeOwer> HomeOwers { get; set; }
        #endregion

        #region 基本属性
        public int? TenantId { get; set; }

        /// <summary>
        /// 单元楼名称
        /// </summary>
        public string BuildingName { get; set; } 
        #endregion
        
    }
}
