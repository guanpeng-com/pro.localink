using System.ComponentModel.DataAnnotations;

namespace DM.AbpZeroTemplate.WebApi.Models
{
    public class GatherDeliveryModel
    {
        /// <summary>
        /// 业主Id
        /// </summary>
        [Required]
        public long HomeOwerId { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int? TenantId { get; set; }

    }
}
