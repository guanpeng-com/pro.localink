using DM.DoorSystem.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Clients
{
    /// <summary>
    /// 小区service
    /// </summary>
    public interface ICommunityClient
    {
        /// <summary>
        ///  创建小区
        /// </summary>
        /// <param name="departName"></param>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        CommunityResponse Create(string departName, string cityCode);

        /// <summary>
        /// 获取小区
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        CommunityResponse Get(string departId);
    }
}
