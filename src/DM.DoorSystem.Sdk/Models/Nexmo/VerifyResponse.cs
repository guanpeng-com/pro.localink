using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Models
{
    public class VerifyResponse
    {
        [JsonProperty("event_id")]
        public string RequestId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("error_text")]
        public string ErrorText { get; set; }
    }
}
