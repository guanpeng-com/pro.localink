using Abp.Authorization.Roles;
using DM.AbpZeroTemplate.Authorization.Users;
using DM.AbpZeroTemplate.DoorSystem.Community;
using DM.AbpZeroTemplate.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.AbpZeroTemplate.Authorization.Roles
{
    /// <summary>
    /// Represents a role in the system.
    /// </summary>
    public class Role : AbpRole<User>
    {
        public Role()
        {
        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {

        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }

        /// <summary>
        /// 角色可以管理的小区ID
        /// </summary>
        public virtual ICollection<long> CommunityIdArray
        {
            get
            {
                List<long> list = new List<long>();
                if (!string.IsNullOrEmpty(CommunityIds))
                {
                    var idStrArray = CommunityIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string idStr in idStrArray)
                    {
                        int id = 0;
                        if (int.TryParse(idStr, out id))
                        {
                            list.Add(id);
                        }
                    }
                }
                return list;
            }
            set
            {
                CommunityIds = String.Join(",", value);
            }
        }
        public virtual string CommunityIds { get; private set; }


    }
}
