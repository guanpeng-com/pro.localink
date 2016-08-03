using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.DoorSystem
{
    [Table("localink_Deliverys")]
    public class Delivery : FullAuditedEntity<long>, IMayHaveTenant
    {
        public Delivery() { }

        public Delivery(int? tenantId, long homeOwerId)
        {
            TenantId = tenantId;
            Title = string.Empty;
            Content = string.Empty;
            HomeOwerId = homeOwerId;
            IsGather = false;
            IsReplace = false;
            Token = GetToken();
        }


        public const int MaxDefaultStringLength = 50;
        public const int MaxContentStringLength = 500;

        public virtual int? TenantId { get; set; }

        [Required]
        [StringLength(MaxDefaultStringLength)]
        public virtual string Title { get; set; }


        [StringLength(MaxContentStringLength)]
        public virtual string Content { get; set; }

        public virtual long HomeOwerId { get; set; }

        [ForeignKey("HomeOwerId")]
        public virtual HomeOwer HomeOwer { get; set; }

        /// <summary>
        /// 是否收取
        /// </summary>
        public virtual bool IsGather { get; set; }

        /// <summary>
        /// 收取时间
        /// </summary>
        public virtual DateTime? GatherTime { get; set; }

        /// <summary>
        /// 是否待收取
        /// </summary>
        public virtual bool IsReplace { get; set; }

        /// <summary>
        /// 待收取业主
        /// </summary>
        public virtual long? ReplaceHomeOwerId { get; set; }

        [ForeignKey("ReplaceHomeOwerId")]
        public virtual HomeOwer ReplaceHomeOwer { get; set; }

        /// <summary>
        /// 收取验证码
        /// </summary>
        public virtual string Token { get; set; }

        private string GetToken()
        {
            Random r = new Random(Clock.Now.Millisecond);
            return r.Next(1000, 9999).ToString();
        }

    }
}
