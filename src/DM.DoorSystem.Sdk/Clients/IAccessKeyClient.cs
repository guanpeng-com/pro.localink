using DM.DoorSystem.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Clients
{
    /// <summary>
    /// 钥匙service
    /// </summary>
    public interface IAccessKeyClient
    {
        /// <summary>
        /// 获取门禁钥匙
        /// </summary>
        /// <param name="pids"></param>
        /// <returns></returns>
        AccessKeyResponse Get(List<string> pids, string userId, string dateStr);

        /// <summary>
        /// 获取门禁钥匙
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        AccessKeyResponse Get(string pid, string userId, string dateStr);
    }
}
