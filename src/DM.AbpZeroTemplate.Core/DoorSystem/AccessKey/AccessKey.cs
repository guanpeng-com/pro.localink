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
        public AccessKey()
        {

        }

        public AccessKey(int? tenantId, long doorId, long homeOwerId, DateTime validity, long communityId)
        {
            TenantId = tenantId;
            DoorId = doorId;
            HomeOwerId = homeOwerId;
            Validity = validity;
            CommunityId = communityId;
        }

        public const int MaxDefaultStringLength = 50;


        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 门禁Id
        /// </summary>
        [Required]
        public virtual long DoorId { get; set; }

        /// <summary>
        /// 业主Id
        /// </summary>
        public virtual long HomeOwerId { get; set; }

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

        [ForeignKey("CommunityId")]
        public virtual Community.Community Community { get; set; }
        public virtual long CommunityId { get; set; }

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
    }
}
