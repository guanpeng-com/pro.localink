using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Models
{
    public class DoorResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public Door Door
        {
            get; set;
        }
    }
}
