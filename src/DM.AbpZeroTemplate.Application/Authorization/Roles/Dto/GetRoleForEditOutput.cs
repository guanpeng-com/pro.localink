using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DM.AbpZeroTemplate.Authorization.Dto;
using DM.AbpZeroTemplate.DoorSystem.Community.Dto;

namespace DM.AbpZeroTemplate.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput : IOutputDto
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }

        public List<CommunityDto> Communities { get; set; }
    }
}