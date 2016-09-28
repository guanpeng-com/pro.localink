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

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        VerifySendResponse SendVerify(string from, string to);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        VerifyResponse Verify(string requestId, string code);

    }
}
