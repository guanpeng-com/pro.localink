using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Models
{
    public class SMSSendResponse
    {
        [JsonProperty("message-count")]
        public string MessageCount { get; set; }
        [JsonProperty("messages")]
        public List<SMSSend> SMSSends
        {
            get; set;
        }
    }
}
