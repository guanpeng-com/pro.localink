using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
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
    /// 地区
    /// </summary>
    [MultiTenancySideAttribute(MultiTenancySides.Host)]
    [Table("localink_Areas")]
    public class Area : Entity<long>, IHasCreationTime
    {
        #region 构造函数
        public Area() {
            
        }

        public Area(long? parentId, string name)
        {
            ParentId = parentId;
            Name = name;
        }
        #endregion

        #region 字段属性
        public const int MaxDefaultStringLength = 50;

        public const string ParentPathSplitString = ",";
        #endregion

        #region 外键
        /// <summary>
        /// 父级Id，M to 1
        /// </summary>
        public virtual long? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Area Parent { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public virtual ICollection<Area> Children { get; set; }
        #endregion

        #region 基本属性
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 父级路径
        /// </summary>
        public virtual string ParentPath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
        #endregion
    }
}
