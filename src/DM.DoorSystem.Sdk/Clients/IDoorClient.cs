using DM.DoorSystem.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Clients
{
    /// <summary>
    /// 门禁service
    /// </summary>
    public interface IDoorClient
    {
        /// <summary>
        ///  创建门禁
        /// </summary>
        /// <param name="departName"></param>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        DoorResponse Create(string pid, string departId, string lockName);

        /// <summary>
        /// 获取门禁
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        DoorResponse Get(string departId);
    }
}
