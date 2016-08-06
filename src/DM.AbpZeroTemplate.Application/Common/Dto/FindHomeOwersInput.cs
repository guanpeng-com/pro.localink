using DM.AbpZeroTemplate.Dto;

namespace DM.AbpZeroTemplate.Common.Dto
{
    public class FindHomeOwersInput : PagedAndFilteredInputDto
    {
        public long? communityId { get; set; }
    }
}