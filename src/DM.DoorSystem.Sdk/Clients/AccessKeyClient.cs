using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DoorSystem.Sdk.Models;

namespace DM.DoorSystem.Sdk.Clients
{
    public class AccessKeyClient : ClientBase, IAccessKeyClient
    {

        public AccessKeyResponse Get(List<string> pids, string userId, string dateStr)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("pid", string.Join(",", pids));
            param.Add("user_id", userId);
            param.Add("validity", dateStr);
            string json = Requestor.DoRequest("/qryKeys", "POST", param);
            return Mapper<AccessKeyResponse>.MapFromJson(json);
        }

        public AccessKeyResponse Get(string pid, string userId, string dateStr)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("pid", pid);
            param.Add("user_id", userId);
            param.Add("validity", dateStr);
            string json = Requestor.DoRequest("/qryKeys", "POST", param);
            return Mapper<AccessKeyResponse>.MapFromJson(json);
        }
    }
}
