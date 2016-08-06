using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DoorSystem.Sdk.Models
{
    public class SMSSend
    {
        [JsonProperty("status")]
        public string Stuats { get; set; }
        [JsonProperty("message-id")]
        public string MessageId { get; set; }
        [JsonProperty("to")]
        public string To { get; set; }
        [JsonProperty("client-ref")]
        public string ClientRef { get; set; }
        [JsonProperty("remaining-balance")]
        public string RemainingBalance { get; set; }
        [JsonProperty("message-price")]
        public string MessagePrice { get; set; }
        [JsonProperty("network")]
        public string Network { get; set; }
        [JsonProperty("error-text")]
        public string ErrorText { get; set; }
    }
}
