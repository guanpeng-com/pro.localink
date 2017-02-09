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

        public Building(int? tenantId, long communityId, string buildingName)
        {
            this.TenantId = tenantId;
            this.CommunityId = communityId;
            this.BuildingName = buildingName;
        }
        #endregion

        #region 外键
        /// <summary>
        /// 小区，M to 1
        /// </summary>
        public long CommunityId { get; set; }
        [ForeignKey("CommunityId")]
        public virtual Community.Community Community { get; set; }

        /// <summary>
        /// 门牌号集合, 1 to M
        /// </summary>
        public virtual ICollection<FlatNumber> FlatNumbers { get; set; }
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
