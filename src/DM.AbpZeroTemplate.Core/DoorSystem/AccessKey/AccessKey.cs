using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using DM.DoorSystem.Sdk.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    [Table("localink_AccessKeys")]
    public class AccessKey : FullAuditedEntity<long>, IMayHaveTenant, IAdminCommunity
    {
        #region 构造函数
        public AccessKey()
        {

        }

        public AccessKey(int? tenantId, Door door, HomeOwer homeOwer, DateTime validity, long communityId)
        {
            TenantId = tenantId;
            HomeOwer = homeOwer;
            Validity = validity;
            CommunityId = communityId;
            Door = door;
        }
        #endregion

        #region 字段属性
        public const int MaxDefaultStringLength = 50;
        #endregion

        #region 外键

        public virtual long DoorId { get; set; }
        [ForeignKey("DoorId")]
        public virtual Door Door { get; set; }

        public virtual long HomeOwerId { get; set; }
        [ForeignKey("HomeOwerId")]
        public virtual HomeOwer HomeOwer { get; set; }
        #endregion

        #region 基本属性
        /// <summary>
        /// 租户ID，冗余字段
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 小区Id，冗余字段
        /// </summary>
        public virtual long CommunityId { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        [Required]
        public virtual DateTime Validity { get; set; }

        /// <summary>
        /// 钥匙串
        /// </summary>
        public virtual string LockId { get; set; }

        /// <summary>
        /// 是否认证
        /// </summary>
        public virtual bool IsAuth { get; private set; }
        #endregion

        #region 方法
        /// <summary>
        /// 申请钥匙
        /// </summary>
        /// <param name="pId">门禁编号</param>
        /// <param name="phone">用户手机号</param>
        /// <param name="validity">钥匙有效期</param>
        public void GetKey(string pId, string phone, DateTime validity)
        {
            if (this.IsAuth)
                return;
            try
            {
                AccessKeyClient akc = new AccessKeyClient();
                var response = akc.Get(pId, phone, validity.ToString("yyyyMMdd"));
                if (response.Code == "0")
                {
                    this.IsAuth = true;
                    this.LockId = response.AccessKeys[0].LockID;
                }
                else
                {
                    this.IsAuth = false;
                }
            }
            catch (Exception)
            {
                this.IsAuth = false;
            }
        }
        #endregion
    }
}
