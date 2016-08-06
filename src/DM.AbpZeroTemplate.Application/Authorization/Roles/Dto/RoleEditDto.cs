using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using System.Collections.Generic;
using DM.AbpZeroTemplate.DoorSystem.Community.Dto;

namespace DM.AbpZeroTemplate.Authorization.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }

        public List<long> CommunityIdArray { get; set; }
    }
}