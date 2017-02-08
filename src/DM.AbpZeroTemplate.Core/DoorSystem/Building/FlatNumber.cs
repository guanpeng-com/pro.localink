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
    /// 门牌号
    /// </summary>
    [Table("localink_FlatNumbers")]
    public class FlatNumber : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {

        #region 构造函数
        public FlatNumber() { }

        public FlatNumber(int? tenantId, long communityId, long buildingId, string flatNo)
        {
            this.TenantId = tenantId;
            this.CommunityId = communityId;
            this.BuildingId = buildingId;
            this.FlatNo = flatNo;
        }
        #endregion

        #region 外键
        /// <summary>
        /// 单元楼
        /// </summary>
        public virtual long BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; }

        public virtual ICollection<HomeOwer> HomeOwers { get; set; }
        #endregion

        /// <summary>
        /// 冗余Building字段
        /// </summary>
        public virtual long CommunityId
        {
            get; set;
        }

        public virtual int? TenantId
        {
            get; set;
        }

        /// <summary>
        /// 门牌号
        /// </summary>
        public virtual string FlatNo { get; set; }

    }
}
