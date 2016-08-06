using DM.AbpZeroTemplate.Dto;

namespace DM.AbpZeroTemplate.Common.Dto
{
    public class FindDoorsInput : PagedAndFilteredInputDto
    {
        public long CommunityId { get; set; }
        public string DoorType { get; set; }
    }
}