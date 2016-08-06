using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DoorSystem.Sdk.Models;
using DM.DoorSystem.Sdk;

namespace DM.DoorSystem.Sdk.Clients
{
    public class SMSClient : ClientBase<NexmoSystem>, ISMSClient
    {
        public SMSSendResponse Send(string from, string to, string text)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("from", from);
            param.Add("to", to);
            param.Add("text", System.Web.HttpUtility.UrlEncode(text, Encoding.UTF8).ToUpper());
            string json = Requestor.DoRequest("/sms/json", "POST", param);
            return Mapper<SMSSendResponse>.MapFromJson(json);
        }

        public string ValidateCode()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return r.Next(1000, 9999).ToString();
        }
    }
}
