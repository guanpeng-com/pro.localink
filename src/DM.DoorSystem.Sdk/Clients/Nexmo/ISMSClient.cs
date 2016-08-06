using DM.DoorSystem.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Clients
{
    /// <summary>
    /// 短信service
    /// </summary>
    public interface ISMSClient
    {
        /// <summary>
        ///  发送短信
        /// </summary>
        /// <param name="from">谁发送</param>
        /// <param name="to">发送给谁</param>
        /// <param name="text">内容</param>
        /// <returns></returns>
        SMSSendResponse Send(string from, string to, string text);

    }
}
