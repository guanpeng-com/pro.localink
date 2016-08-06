using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DoorSystem.Sdk.Models;

namespace DM.DoorSystem.Sdk.Clients
{
    public class DoorClient : ClientBase<DoorSystem>, IDoorClient
    {
        public DoorResponse Create(string pid, string departId, string lockName)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("pid", pid);
            param.Add("departid", departId);
            param.Add("install_lock_name", lockName);
            string json = Requestor.DoRequest("/installLock", "POST", param);
            return Mapper<DoorResponse>.MapFromJson(json);
        }

        public DoorResponse Get(string pid)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("pid", pid);
            string json = Requestor.DoRequest("/queryDevice", "POST", param);
            return Mapper<DoorResponse>.MapFromJson(json);
        }
    }
}
