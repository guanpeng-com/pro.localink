using System.ComponentModel.DataAnnotations;

namespace DM.AbpZeroTemplate.Web.Models.Account
{
    public class ResetPasswordFormViewModel
    {
        /// <summary>
        /// Encrypted user id.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Encrypted tenant id.
        /// </summary>
        [Required]
        public string TenantId { get; set; }

        [Required]
        public string ResetCode { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}