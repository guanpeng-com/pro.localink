using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.DoorSystem.Sdk.Models;

namespace DM.DoorSystem.Sdk.Clients
{
    public class CommunityClient : ClientBase, ICommunityClient
    {
        public CommunityResponse Create(string departName, string cityCode)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("depart_name", departName);
            param.Add("city_code", cityCode);
            string json = Requestor.DoRequest("/addCommunity", "POST", param);
            return Mapper<CommunityResponse>.MapFromJson(json);
        }

        public CommunityResponse Get(string departId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("departId", departId);
            string json = Requestor.DoRequest("/getCommunity", "POST", param);
            return Mapper<CommunityResponse>.MapFromJson(json);
        }
    }
}
